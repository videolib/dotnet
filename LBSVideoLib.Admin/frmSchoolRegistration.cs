using LBFVideoLib.Common;
using LBFVideoLib.Common.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using IWshRuntimeLibrary;

namespace LBFVideoLib.Admin {
  public partial class frmSchoolRegistration : Form {
    //private string _clientInfoFilePath = Path.Combine(Directory.GetCurrentDirectory(), "clientInfo-encrypt.txt");
    private string _clientTargetPath = "";// Path.Combine(Directory.GetCurrentDirectory(), "clientTarget");
    private string _sourceVideoFolderPath = "";
    private string _clientDistributionRootPath = "";
    private string _clientInfoFileName = "";

    List<SchoolClass> _classList = new List<SchoolClass>();
    List<Series> seriesList = new List<Series>();
    List<Subject> _subjectList = new List<Subject>();
    List<Book> bookList = new List<Book>();

    List<ClassFB> regInfoFB = new List<ClassFB>();


    public frmSchoolRegistration() {
      InitializeComponent();
    }

    private void frmSchoolRegistration_Load(object sender, EventArgs e) {
      // read configuration information
      _sourceVideoFolderPath = ConfigHelper.SourceVideoFolderPath;
      _clientTargetPath = _clientDistributionRootPath = ConfigHelper.ClientDistributionTargetRootPath;
      _clientInfoFileName = ConfigHelper.ClientInfoFileName;
      //    this.lblSessionYears.Text = string.Format(ConfigHelper.SessionYears, ConfigHelper.SessionYears);


      InitializeRegistrationForm();
    }

    private void btnRegister_Click(object sender, EventArgs e) {

      if (ValidateRegistrationForm() == false) {
        return;
      }



      #region Create Folder Structure

      // Copy encrypted client info json file to target location.
      if (Directory.Exists(_clientDistributionRootPath) == false) {
        Directory.CreateDirectory(_clientDistributionRootPath);
      }

      // Define client pacakge root folder path i.e. school code
      string clientSchoolCodeFolderPath = Path.Combine(_clientDistributionRootPath, txtSchoolCode.Text.Trim());
      if (Directory.Exists(clientSchoolCodeFolderPath) == false) {
        Directory.CreateDirectory(clientSchoolCodeFolderPath);
      }

      // Delete all old directory and files
      if (Directory.Exists(clientSchoolCodeFolderPath)) {
        string[] oldClientFiles = Directory.GetDirectories(clientSchoolCodeFolderPath);
        for (int i = 0; i < oldClientFiles.Length - 1; i++) {
          //System.IO.File.Delete(Path.Combine(clientSchoolCodeFolderPath, oldClientFiles[i]));
          System.IO.Directory.Delete(oldClientFiles[i], true);
        }
      }

      // Define client pacakge folder path i.e. pacakage
      string clientPacakgeFolderPath = Path.Combine(clientSchoolCodeFolderPath, "Package");
      if (Directory.Exists(clientPacakgeFolderPath) == false) {
        Directory.CreateDirectory(clientPacakgeFolderPath);
      }


      // Define client video folder path i.e. SchoolCode_City_LBFVideos
      string clientVideoFolderPath = Path.Combine(clientSchoolCodeFolderPath, string.Format("{0}_{1}_LBFVideos", txtSchoolCode.Text.Trim(), txtSchoolCity.Text.Trim()));
      if (Directory.Exists(clientVideoFolderPath) == false) {
        Directory.CreateDirectory(clientVideoFolderPath);
      }
      #endregion

      // Copy client distribution
      if (Directory.Exists(ConfigHelper.ClientDistributionPath)) {
        string[] clientDistributionFiles = Directory.GetFiles(ConfigHelper.ClientDistributionPath);
        for (int i = 0; i < clientDistributionFiles.Length; i++) {
          System.IO.File.Copy(Path.Combine(ConfigHelper.ClientDistributionPath, clientDistributionFiles[i]), Path.Combine(clientPacakgeFolderPath, Path.GetFileName(clientDistributionFiles[i])), true);
        }

        // Create shortcut of exe.
        WshShellClass shell = new WshShellClass();
        IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(Path.Combine(clientSchoolCodeFolderPath, "LBSVideoLib.Client.exe.lnk"));
        shortcut.TargetPath = Path.Combine(clientPacakgeFolderPath, "LBSVideoLib.Client.exe");
        // add Description of Short cut
        shortcut.Description = "Run this exe to play LBF Video Library.";
        // save it / create
        shortcut.Save();
      }
      else {
        MessageBox.Show("Client distribution doesn't find on specified path.", "Error", MessageBoxButtons.OK);
      }

      // Set client email, password and license date in client info class.
      ClientInfo clientInfo = new ClientInfo();
      clientInfo.EmailId = txtEmailId.Text.ToLower().Trim();
      clientInfo.Password = txtPwd.Text.Trim();
      clientInfo.ExpiryDate = LicenseHelper.GetExpiryDateBySessionString(cmbSchoolSession.SelectedItem.ToString());
      clientInfo.SessionString = cmbSchoolSession.SelectedItem.ToString();
      clientInfo.SchoolId = this.txtSchoolCode.Text.Trim();
      clientInfo.SchoolName = this.txtSchoolName.Text.Trim();
      clientInfo.SchoolCity = txtSchoolCity.Text.Trim();

      // Generate client info json file and encrypt it.
      Cryptograph.EncryptObject(clientInfo, Path.Combine(clientPacakgeFolderPath, _clientInfoFileName));

      //  File.Move(_clientInfoFilePath, Path.Combine(_clientTargetPath, Path.GetFileName(_clientInfoFilePath)));

      for (int i = 0; i < chkListBooks.CheckedItems.Count; i++) {
        Book selectedBook = (chkListBooks.CheckedItems[i]) as Book;

        string[] selectedBookVideos = Directory.GetFiles(selectedBook.BookId);
        foreach (string selectedBookVideo in selectedBookVideos) {
          string clientTargetVideoPath = Path.Combine(clientVideoFolderPath, selectedBook.ClassName);

          clientTargetVideoPath = Path.Combine(clientTargetVideoPath, selectedBook.SeriesName);
          clientTargetVideoPath = Path.Combine(clientTargetVideoPath, selectedBook.SubjectName);
          clientTargetVideoPath = Path.Combine(clientTargetVideoPath, selectedBook.BookName);
          if (Directory.Exists(clientTargetVideoPath) == false) {
            Directory.CreateDirectory(clientTargetVideoPath);
          }
          clientTargetVideoPath = Path.Combine(clientTargetVideoPath, Path.GetFileName(selectedBookVideo));

          Cryptograph.EncryptFile(selectedBookVideo, clientTargetVideoPath);
        }
      }

      // Save data on firebase
      SaveRegDataOnFireBase();

      // Copy client project bin folder to target location.
      MessageBox.Show("Registraion completed sucessfully.", "Info", MessageBoxButtons.OK);

      InitializeRegistrationForm();

    }

    #region Check List Item Check Events
    private void chkListClass_ItemCheck(object sender, ItemCheckEventArgs e) {
      SchoolClass selectedClass = (chkListClass.Items[e.Index] as SchoolClass);
      updateSeriesListBinding(e.NewValue, selectedClass, e.Index);
    }

    private void chkListSeries_ItemCheck(object sender, ItemCheckEventArgs e) {
      Series selectedSeries = (chkListSeries.Items[e.Index] as Series);
      updateSubjectListBinding(e.NewValue, selectedSeries);
    }

    private void chkListSubject_ItemCheck(object sender, ItemCheckEventArgs e) {
      Subject selectedSubject = (chkListSubject.Items[e.Index] as Subject);
      updateBookListBinding(e.NewValue, selectedSubject);
    }

    private void chkListBook_ItemCheck(object sender, ItemCheckEventArgs e) {
      Book selectedBook = (chkListBooks.Items[e.Index] as Book);
      if (e.NewValue == CheckState.Checked) {
        selectedBook.Selected = true;
      }
      else if (e.NewValue == CheckState.Unchecked) {
        selectedBook.Selected = false;
      }
    }
    #endregion

    #region Update Check List Item Binding Methods
    private void updateSeriesListBinding(CheckState checkedState, SchoolClass selectedClass, int selectedClassIndex) {
      List<Series> removedSeriesList = new List<Series>();
      // On each item check/un-check fill series
      if (checkedState == CheckState.Checked) {
        // add series
        //SchoolClass selectedClass = (chkListClass.Items[e.Index] as SchoolClass);
        selectedClass.Selected = true;
        string[] seriesFolderList = Directory.GetDirectories(selectedClass.ClassId); //Directory.GetDirectories(Path.Combine(_sourceFolderPath, selectedClassName));
        for (int i = 0; i < seriesFolderList.Length; i++) {
          //chkListSeries.Items.Add(seriesList[i]);
          Series series = new Series();
          series.SeriesId = seriesFolderList[i];
          series.SeriesName = Path.GetFileName(seriesFolderList[i]);
          series.ClassName = selectedClass.ClassName;
          seriesList.Add(series);
        }
      }
      else if (checkedState == CheckState.Unchecked) {
        selectedClass.Selected = false;
        // remove series
        SchoolClass selectedClassName = (chkListClass.Items[selectedClassIndex] as SchoolClass);
        string[] seriesFolderList = Directory.GetDirectories(selectedClassName.ClassId);//Directory.GetDirectories(Path.Combine(_sourceFolderPath, selectedClassName));

        seriesList = seriesList.Where(b => {
          if (b.ClassName != selectedClassName.ClassName) {
            return true;
          }
          else {
            removedSeriesList.Add(b);
            return false;
          }
        }
        ).ToList<Series>();

      }

   ((ListBox)this.chkListSeries).DataSource = null;
      ((ListBox)this.chkListSeries).DataSource = seriesList;
      ((ListBox)this.chkListSeries).DisplayMember = "SeriesName";
      ((ListBox)this.chkListSeries).ValueMember = "Selected";


      for (int i = 0; i < seriesList.Count; i++) {
        if (seriesList[i].Selected) {
          this.chkListSeries.SetItemChecked(i, true);
        }
      }

      foreach (Series removedSeries in removedSeriesList) {
        updateSubjectListBinding(CheckState.Unchecked, removedSeries);
      }
    }

    private void updateSubjectListBinding(CheckState checkedState, Series selectedSeries) {
      List<Subject> removedSubjectList = new List<Subject>();
      // On each item check/un-check fill series
      if (checkedState == CheckState.Checked) {
        // add series
        string[] subjectFolderList = Directory.GetDirectories(selectedSeries.SeriesId); // Directory.GetDirectories(Path.Combine(_sourceFolderPath, Path.Combine(selectedSeries.ClassName, selectedSeries.SeriesName)));
        for (int i = 0; i < subjectFolderList.Length; i++) {
          //chkListSeries.Items.Add(seriesList[i]);
          Subject subject = new Subject();
          subject.SubjectId = subjectFolderList[i];
          subject.SubjectName = Path.GetFileName(subjectFolderList[i]);
          subject.SeriesName = selectedSeries.SeriesName;
          subject.ClassName = selectedSeries.ClassName;
          _subjectList.Add(subject);
        }
        selectedSeries.Selected = true;

      }
      else if (checkedState == CheckState.Unchecked) {
        selectedSeries.Selected = false;

        // remove series
        string[] subjectFolderList = Directory.GetDirectories(Path.Combine(_sourceVideoFolderPath, Path.Combine(selectedSeries.ClassName, selectedSeries.SeriesName)));
        _subjectList = _subjectList.Where(b => {
          if (b.SeriesName != selectedSeries.SeriesName) {
            return true;
          }
          else {
            removedSubjectList.Add(b);
            return false;
          }
        }
        ).ToList<Subject>();

      }



        ((ListBox)this.chkListSubject).DataSource = null;
      ((ListBox)this.chkListSubject).DataSource = _subjectList;
      ((ListBox)this.chkListSubject).DisplayMember = "SubjectName";
      ((ListBox)this.chkListSubject).ValueMember = "Selected";


      foreach (Subject removedSeries in removedSubjectList) {
        updateBookListBinding(CheckState.Unchecked, removedSeries);
      }

      for (int i = 0; i < _subjectList.Count; i++) {
        if (_subjectList[i].Selected) {
          this.chkListSubject.SetItemChecked(i, true);
        }
      }

    }

    private void updateBookListBinding(CheckState checkedState, Subject selectedSubject) {
      // On each item check/un-check fill series
      if (checkedState == CheckState.Checked) {
        // add series
        string[] bookFolderList = Directory.GetDirectories(selectedSubject.SubjectId); // Directory.GetDirectories(Path.Combine(_sourceFolderPath, Path.Combine(selectedSeries.ClassName, selectedSeries.SeriesName)));
        for (int i = 0; i < bookFolderList.Length; i++) {
          //chkListSeries.Items.Add(seriesList[i]);
          Book book = new Book();
          book.BookId = bookFolderList[i];
          book.BookName = Path.GetFileName(bookFolderList[i]);
          book.SubjectName = selectedSubject.SubjectName;
          book.ClassName = selectedSubject.ClassName;
          book.SeriesName = selectedSubject.SeriesName;
          bookList.Add(book);
        }
        selectedSubject.Selected = true;

      }
      else if (checkedState == CheckState.Unchecked) {
        selectedSubject.Selected = false;

        // remove series
        string[] bookFolderList = Directory.GetDirectories(Path.Combine(_sourceVideoFolderPath, Path.Combine(Path.Combine(selectedSubject.ClassName, selectedSubject.SeriesName), selectedSubject.SubjectName)));
        bookList = bookList.Where(b => {
          if (b.SubjectName != selectedSubject.SubjectName) {
            return true;
          }
          else {
            return false;
          }
        }
        ).ToList<Book>();

      }



          ((ListBox)this.chkListBooks).DataSource = null;
      ((ListBox)this.chkListBooks).DataSource = bookList;
      ((ListBox)this.chkListBooks).DisplayMember = "BookName";
      ((ListBox)this.chkListBooks).ValueMember = "Selected";


      for (int i = 0; i < bookList.Count; i++) {
        if (bookList[i].Selected) {
          this.chkListBooks.SetItemChecked(i, true);
        }
      }
    }
    #endregion

    private void InitializeRegistrationForm() {
      txtEmailId.Text = "";
      txtPwd.Text = "";
      txtSchoolCity.Text = "";
      txtSchoolCode.Text = "";
      txtSchoolName.Text = "";
      cmbSchoolSession.DataSource = LicenseHelper.GetSessionList();
      cmbSchoolSession.SelectedIndex = -1;
      // Read all folders to fill classes
      string[] classNameList = Directory.GetDirectories(_sourceVideoFolderPath);

      for (int i = 0; i < classNameList.Length; i++) {
        SchoolClass schoolClass = new SchoolClass();
        schoolClass.ClassId = classNameList[i];
        schoolClass.ClassName = Path.GetFileName(classNameList[i]);
        _classList.Add(schoolClass);
      }

 ((ListBox)this.chkListClass).DataSource = null;

      ((ListBox)this.chkListClass).DataSource = _classList;
      ((ListBox)this.chkListClass).DisplayMember = "ClassName";
      ((ListBox)this.chkListClass).ValueMember = "Selected";
      chkListSeries.DataSource = null;
      chkListSubject.DataSource = null;
      chkListBooks.DataSource = null;


    }

    private bool ValidateRegistrationForm() {
      if (txtEmailId.Text.Trim() == string.Empty) {
        MessageBox.Show("Email Id is required.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return false;
      }

      if (txtPwd.Text.Trim() == string.Empty) {
        MessageBox.Show("Password is required.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return false;
      }

      if (txtSchoolCity.Text.Trim() == string.Empty) {
        MessageBox.Show("School city is required.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return false;
      }

      if (txtSchoolCode.Text.Trim() == string.Empty) {
        MessageBox.Show("School code is required.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return false;
      }

      if (txtSchoolName.Text.Trim() == string.Empty) {
        MessageBox.Show("School name is required.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return false;
      }

      if (cmbSchoolSession.SelectedIndex < 0) {
        MessageBox.Show("Please select a valid session.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return false;
      }

      if (chkListBooks.CheckedItems.Count < 1) {
        MessageBox.Show("Please select atleast one book.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return false;
      }

      return true;

    }

    private void SaveRegDataOnFireBase() {
      RegInfoFB info = new RegInfoFB();
      info.RegDate = DateTime.Now.ToString();
      info.LoginEmail = txtEmailId.Text;
      info.Password = txtPwd.Text;
      info.SchoolName = txtSchoolName.Text;
      info.City = txtSchoolCity.Text;
      info.Session = cmbSchoolSession.SelectedText;
      info.Classes = new List<ClassFB>();

      for (int i = 0; i < chkListBooks.CheckedItems.Count; i++) {
        Book selectedBook = (chkListBooks.CheckedItems[i]) as Book;

        ClassFB selectedFBClass = info.Classes.Find(k => k.Name == selectedBook.ClassName);
        if (selectedFBClass == null) {
          selectedFBClass = new ClassFB();
          selectedFBClass.Name = selectedBook.ClassName;
          info.Classes.Add(selectedFBClass);
        }
        SeriesFB selectedFBSeries = selectedFBClass.Series.Find(k => k.Name == selectedBook.SeriesName);

        if (selectedFBSeries == null) {
          selectedFBSeries = new SeriesFB();
          selectedFBSeries.Name = selectedBook.SeriesName;
          selectedFBClass.Series.Add(selectedFBSeries);
        }

        SubjectFB selectedFBSubject = selectedFBSeries.Subjects.Find(k => k.Name == selectedBook.SubjectName);

        if (selectedFBSubject == null) {
          selectedFBSubject = new SubjectFB();
          selectedFBSubject.Name = selectedBook.SubjectName;
          selectedFBSeries.Subjects.Add(selectedFBSubject);
        }

        BookFB selectedFBBook = new BookFB();
        selectedFBBook.Name = selectedBook.BookName;
        selectedFBSubject.Books.Add(selectedFBBook);

      }    

      string jsonString1 = JsonHelper.ParseObjectToJSON<RegInfoFB>(info);
      FirebaseHelper.PostData(jsonString1, txtSchoolCode.Text);
    }
  }
}


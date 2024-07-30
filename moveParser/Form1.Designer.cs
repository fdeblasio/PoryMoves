namespace moveParser
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            //Variables
            int leftmostX = 15;
            int width = 600;
            int chkHeight = 21;
            int gBoxWidth = 180;
            int gBoxHeight = 375;
            int columnWidth = 150;
            int btnHeight = 27;
            int listHeight = 220;
            int metaY = 14;
            int labelY = 153;
            int selectAllX = 5;
            int selectAllY = 240;
            int firstCheckY = 275;
            int secondCheckY = 300;
            int exportX = 13;
            int exportY = 325;

            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.btnLoadFromSerebii = new System.Windows.Forms.Button();
            this.pbar1 = new System.Windows.Forms.ProgressBar();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.lblLoading = new System.Windows.Forms.Label();
            this.cmbGeneration = new System.Windows.Forms.ComboBox();
            this.btnWriteLvlLearnsets = new System.Windows.Forms.Button();
            this.bwrkExportLvl = new System.ComponentModel.BackgroundWorker();
            this.cListTMMoves = new System.Windows.Forms.CheckedListBox();
            this.gBoxOptionsTM = new System.Windows.Forms.GroupBox();
            this.btnTM_All = new System.Windows.Forms.Button();
            this.chkTM_IncludeEgg = new System.Windows.Forms.CheckBox();
            this.btnExportTM = new System.Windows.Forms.Button();
            this.chkTM_IncludeLvl = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkLvl_PreEvo = new System.Windows.Forms.CheckBox();
            this.btnLvl_All = new System.Windows.Forms.Button();
            this.cListLevelUp = new System.Windows.Forms.CheckedListBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnEgg_All = new System.Windows.Forms.Button();
            this.chkEgg_IncludeTeach = new System.Windows.Forms.CheckBox();
            this.btnExportEgg = new System.Windows.Forms.Button();
            this.chkEgg_IncludeLvl = new System.Windows.Forms.CheckBox();
            this.cListEggMoves = new System.Windows.Forms.CheckedListBox();
            this.bwrkExportTM = new System.ComponentModel.BackgroundWorker();
            this.bwrkExportEgg = new System.ComponentModel.BackgroundWorker();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.chkGeneral_MewExclusiveTutor = new System.Windows.Forms.CheckBox();
            this.chkVanillaMode = new System.Windows.Forms.CheckBox();
            this.btnOpenOutputFolder = new System.Windows.Forms.Button();
            this.btnOpenInputFolder = new System.Windows.Forms.Button();
            this.btnExportAll = new System.Windows.Forms.Button();
            this.bwrkExportAll = new System.ComponentModel.BackgroundWorker();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.gBoxOptionsTM.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            //
            // lblLoading
            //
            this.lblLoading.Location = new System.Drawing.Point(12, metaY);
            this.lblLoading.Name = "lblLoading";
            this.lblLoading.Size = new System.Drawing.Size(340, 32);
            this.lblLoading.TabIndex = 2;
            this.lblLoading.Text = "Welcome!";
            this.lblLoading.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // cmbGeneration
            //
            this.cmbGeneration.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGeneration.FormattingEnabled = true;
            this.cmbGeneration.Location = new System.Drawing.Point(285, 17);
            this.cmbGeneration.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cmbGeneration.Name = "cmbGeneration";
            this.cmbGeneration.Size = new System.Drawing.Size(55, 24);
            this.cmbGeneration.TabIndex = 5;
            this.cmbGeneration.Visible = false;
            //
            // btnLoadFromSerebii
            //
            this.btnLoadFromSerebii.Location = new System.Drawing.Point(350, metaY);
            this.btnLoadFromSerebii.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnLoadFromSerebii.Name = "btnLoadFromSerebii";
            this.btnLoadFromSerebii.Size = new System.Drawing.Size(137, 34);
            this.btnLoadFromSerebii.TabIndex = 0;
            this.btnLoadFromSerebii.Text = "Load from Internet";
            this.btnLoadFromSerebii.UseVisualStyleBackColor = true;
            this.btnLoadFromSerebii.Visible = false;
            this.btnLoadFromSerebii.Click += new System.EventHandler(this.btnLoadFromSerebii_Click);
            //
            // btnOpenInputFolder
            //
            this.btnOpenInputFolder.Location = new System.Drawing.Point(490, metaY);
            this.btnOpenInputFolder.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnOpenInputFolder.Name = "btnOpenInputFolder";
            this.btnOpenInputFolder.Size = new System.Drawing.Size(137, 34);
            this.btnOpenInputFolder.TabIndex = 23;
            this.btnOpenInputFolder.Text = "Open input folder";
            this.btnOpenInputFolder.UseVisualStyleBackColor = true;
            this.btnOpenInputFolder.Click += new System.EventHandler(this.btnOpenInputFolder_Click);
            //
            // btnOpenOutputFolder
            //
            this.btnOpenOutputFolder.Location = new System.Drawing.Point(630, metaY);
            this.btnOpenOutputFolder.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnOpenOutputFolder.Name = "btnOpenOutputFolder";
            this.btnOpenOutputFolder.Size = new System.Drawing.Size(137, 34);
            this.btnOpenOutputFolder.TabIndex = 22;
            this.btnOpenOutputFolder.Text = "Open output folder";
            this.btnOpenOutputFolder.UseVisualStyleBackColor = true;
            this.btnOpenOutputFolder.Click += new System.EventHandler(this.btnOpenOutputFolder_Click);
            //
            // pbar1
            //
            this.pbar1.Location = new System.Drawing.Point(leftmostX, 57);
            this.pbar1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pbar1.Name = "pbar1";
            this.pbar1.Size = new System.Drawing.Size(width, 23);
            this.pbar1.TabIndex = 1;
            //
            // backgroundWorker1
            //
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            //
            // groupBox3
            //
            this.groupBox3.Controls.Add(this.chkGeneral_MewExclusiveTutor);
            this.groupBox3.Controls.Add(this.chkVanillaMode);
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(leftmostX, 86);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox3.Size = new System.Drawing.Size(width, 60);
            this.groupBox3.TabIndex = 20;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "General Options";
            //
            // chkVanillaMode
            //
            this.chkVanillaMode.AutoSize = true;
            this.chkVanillaMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkVanillaMode.Location = new System.Drawing.Point(7, 22);
            this.chkVanillaMode.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkVanillaMode.Name = "chkVanillaMode";
            this.chkVanillaMode.Size = new System.Drawing.Size(111, chkHeight);
            this.chkVanillaMode.TabIndex = 18;
            this.chkVanillaMode.Text = "Vanilla Mode";
            this.toolTip1.SetToolTip(this.chkVanillaMode, resources.GetString("chkVanillaMode.ToolTip"));
            this.chkVanillaMode.UseVisualStyleBackColor = true;
            //
            // chkGeneral_MewExclusiveTutor
            //
            this.chkGeneral_MewExclusiveTutor.AutoSize = true;
            this.chkGeneral_MewExclusiveTutor.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkGeneral_MewExclusiveTutor.Location = new System.Drawing.Point(124, 22);
            this.chkGeneral_MewExclusiveTutor.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkGeneral_MewExclusiveTutor.Name = "chkGeneral_MewExclusiveTutor";
            this.chkGeneral_MewExclusiveTutor.Size = new System.Drawing.Size(265, chkHeight);
            this.chkGeneral_MewExclusiveTutor.TabIndex = 19;
            this.chkGeneral_MewExclusiveTutor.Text = "Mew can learn exclusive Moves";
            this.toolTip1.SetToolTip(this.chkGeneral_MewExclusiveTutor, resources.GetString("chkGeneral_MewExclusiveTutor.ToolTip"));
            //
            // groupBox1
            //
            this.groupBox1.Controls.Add(this.chkLvl_PreEvo);
            this.groupBox1.Controls.Add(this.btnLvl_All);
            this.groupBox1.Controls.Add(this.cListLevelUp);
            this.groupBox1.Controls.Add(this.btnWriteLvlLearnsets);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(leftmostX, labelY);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Size = new System.Drawing.Size(gBoxWidth, gBoxHeight);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Level Up";
            //
            // chkLvl_PreEvo
            //
            this.chkLvl_PreEvo.AutoSize = true;
            this.chkLvl_PreEvo.Checked = true;
            this.chkLvl_PreEvo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkLvl_PreEvo.Location = new System.Drawing.Point(13, firstCheckY);
            this.chkLvl_PreEvo.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkLvl_PreEvo.Name = "chkLvl_PreEvo";
            this.chkLvl_PreEvo.Size = new System.Drawing.Size(175, chkHeight);
            this.chkLvl_PreEvo.TabIndex = 19;
            this.chkLvl_PreEvo.Text = "Pre-Evo moves";
            this.chkLvl_PreEvo.UseVisualStyleBackColor = true;
            //
            // btnLvl_All
            //
            this.btnLvl_All.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLvl_All.Location = new System.Drawing.Point(selectAllX, selectAllY);
            this.btnLvl_All.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnLvl_All.Name = "btnLvl_All";
            this.btnLvl_All.Size = new System.Drawing.Size(columnWidth, btnHeight);
            this.btnLvl_All.TabIndex = 18;
            this.btnLvl_All.Text = "Select All";
            this.btnLvl_All.UseVisualStyleBackColor = true;
            this.btnLvl_All.Click += new System.EventHandler(this.btnLvl_All_Click);
            //
            // cListLevelUp
            //
            this.cListLevelUp.CheckOnClick = true;
            this.cListLevelUp.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cListLevelUp.FormattingEnabled = true;
            this.cListLevelUp.Items.AddRange(new object[] {
            "SWSH",
            "USUM"});
            this.cListLevelUp.Location = new System.Drawing.Point(5, 25);
            this.cListLevelUp.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cListLevelUp.Name = "cListLevelUp";
            this.cListLevelUp.Size = new System.Drawing.Size(columnWidth, listHeight);
            this.cListLevelUp.TabIndex = 12;
            this.chkGeneral_MewExclusiveTutor.UseVisualStyleBackColor = true;
            //
            // btnWriteLvlLearnsets
            //
            this.btnWriteLvlLearnsets.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnWriteLvlLearnsets.Location = new System.Drawing.Point(exportX, exportY);
            this.btnWriteLvlLearnsets.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnWriteLvlLearnsets.Name = "btnWriteLvlLearnsets";
            this.btnWriteLvlLearnsets.Size = new System.Drawing.Size(columnWidth, btnHeight);
            this.btnWriteLvlLearnsets.TabIndex = 7;
            this.btnWriteLvlLearnsets.Text = "Export";
            this.btnWriteLvlLearnsets.UseVisualStyleBackColor = true;
            this.btnWriteLvlLearnsets.Click += new System.EventHandler(this.btnWriteLvlLearnsets_Click);
            //
            // bwrkExportLvl
            //
            this.bwrkExportLvl.WorkerReportsProgress = true;
            this.bwrkExportLvl.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bwrkExportLvl_DoWork);
            this.bwrkExportLvl.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            //
            // cListTMMoves
            //
            this.cListTMMoves.CheckOnClick = true;
            this.cListTMMoves.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cListTMMoves.FormattingEnabled = true;
            this.cListTMMoves.Items.AddRange(new object[] {
            "SV",
            "SWSH",
            "USUM"});
            this.cListTMMoves.Location = new System.Drawing.Point(5, 25);
            this.cListTMMoves.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cListTMMoves.Name = "cListTMMoves";
            this.cListTMMoves.Size = new System.Drawing.Size(columnWidth, listHeight);
            this.cListTMMoves.TabIndex = 12;
            //
            // gBoxOptionsTM
            //
            this.gBoxOptionsTM.Controls.Add(this.btnTM_All);
            this.gBoxOptionsTM.Controls.Add(this.chkTM_IncludeEgg);
            this.gBoxOptionsTM.Controls.Add(this.btnExportTM);
            this.gBoxOptionsTM.Controls.Add(this.chkTM_IncludeLvl);
            this.gBoxOptionsTM.Controls.Add(this.cListTMMoves);
            this.gBoxOptionsTM.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gBoxOptionsTM.Location = new System.Drawing.Point(200, labelY);
            this.gBoxOptionsTM.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gBoxOptionsTM.Name = "gBoxOptionsTM";
            this.gBoxOptionsTM.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gBoxOptionsTM.Size = new System.Drawing.Size(gBoxWidth, gBoxHeight);
            this.gBoxOptionsTM.TabIndex = 13;
            this.gBoxOptionsTM.TabStop = false;
            this.gBoxOptionsTM.Text = "Teachable";
            //
            // btnTM_All
            //
            this.btnTM_All.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTM_All.Location = new System.Drawing.Point(selectAllX, selectAllY);
            this.btnTM_All.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnTM_All.Name = "btnTM_All";
            this.btnTM_All.Size = new System.Drawing.Size(columnWidth, btnHeight);
            this.btnTM_All.TabIndex = 19;
            this.btnTM_All.Text = "Select All";
            this.btnTM_All.UseVisualStyleBackColor = true;
            this.btnTM_All.Click += new System.EventHandler(this.btnTM_All_Click);
            //
            // chkTM_IncludeLvl
            //
            this.chkTM_IncludeLvl.AutoSize = true;
            this.chkTM_IncludeLvl.Checked = true;
            this.chkTM_IncludeLvl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkTM_IncludeLvl.Location = new System.Drawing.Point(7, firstCheckY);
            this.chkTM_IncludeLvl.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkTM_IncludeLvl.Name = "chkTM_IncludeLvl";
            this.chkTM_IncludeLvl.Size = new System.Drawing.Size(columnWidth, chkHeight);
            this.chkTM_IncludeLvl.TabIndex = 13;
            this.chkTM_IncludeLvl.Text = "Level Up";
            this.chkTM_IncludeLvl.UseVisualStyleBackColor = true;
            //
            // chkTM_IncludeEgg
            //
            this.chkTM_IncludeEgg.AutoSize = true;
            this.chkTM_IncludeEgg.Checked = true;
            this.chkTM_IncludeEgg.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkTM_IncludeEgg.Location = new System.Drawing.Point(7, secondCheckY);
            this.chkTM_IncludeEgg.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkTM_IncludeEgg.Name = "chkTM_IncludeEgg";
            this.chkTM_IncludeEgg.Size = new System.Drawing.Size(149, chkHeight);
            this.chkTM_IncludeEgg.TabIndex = 15;
            this.chkTM_IncludeEgg.Text = "Egg";
            this.chkTM_IncludeEgg.UseVisualStyleBackColor = true;
            //
            // btnExportTM
            //
            this.btnExportTM.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExportTM.Location = new System.Drawing.Point(exportX, exportY);
            this.btnExportTM.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnExportTM.Name = "btnExportTM";
            this.btnExportTM.Size = new System.Drawing.Size(columnWidth, btnHeight);
            this.btnExportTM.TabIndex = 14;
            this.btnExportTM.Text = "Export";
            this.btnExportTM.UseVisualStyleBackColor = true;
            this.btnExportTM.Click += new System.EventHandler(this.btnExportTM_Click);
            //
            // groupBox2
            //
            this.groupBox2.Controls.Add(this.btnEgg_All);
            this.groupBox2.Controls.Add(this.chkEgg_IncludeTeach);
            this.groupBox2.Controls.Add(this.btnExportEgg);
            this.groupBox2.Controls.Add(this.chkEgg_IncludeLvl);
            this.groupBox2.Controls.Add(this.cListEggMoves);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(385, labelY);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox2.Size = new System.Drawing.Size(gBoxWidth, gBoxHeight);
            this.groupBox2.TabIndex = 17;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Egg";
            //
            // btnEgg_All
            //
            this.btnEgg_All.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEgg_All.Location = new System.Drawing.Point(selectAllX, selectAllY);
            this.btnEgg_All.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnEgg_All.Name = "btnEgg_All";
            this.btnEgg_All.Size = new System.Drawing.Size(columnWidth, btnHeight);
            this.btnEgg_All.TabIndex = 20;
            this.btnEgg_All.Text = "Select All";
            this.btnEgg_All.UseVisualStyleBackColor = true;
            this.btnEgg_All.Click += new System.EventHandler(this.btnEgg_All_Click);
            //
            // chkEgg_IncludeLvl
            //
            this.chkEgg_IncludeLvl.AutoSize = true;
            this.chkEgg_IncludeLvl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkEgg_IncludeLvl.Location = new System.Drawing.Point(5, firstCheckY);
            this.chkEgg_IncludeLvl.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkEgg_IncludeLvl.Name = "chkEgg_IncludeLvl";
            this.chkEgg_IncludeLvl.Size = new System.Drawing.Size(columnWidth, chkHeight);
            this.chkEgg_IncludeLvl.TabIndex = 13;
            this.chkEgg_IncludeLvl.Text = "Level Up";
            this.chkEgg_IncludeLvl.UseVisualStyleBackColor = true;
            //
            // chkEgg_IncludeTeach
            //
            this.chkEgg_IncludeTeach.AutoSize = true;
            this.chkEgg_IncludeTeach.Checked = true;
            this.chkEgg_IncludeTeach.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkEgg_IncludeTeach.Location = new System.Drawing.Point(5, secondCheckY);
            this.chkEgg_IncludeTeach.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkEgg_IncludeTeach.Name = "chkEgg_IncludeTeach";
            this.chkEgg_IncludeTeach.Size = new System.Drawing.Size(144, chkHeight);
            this.chkEgg_IncludeTeach.TabIndex = 15;
            this.chkEgg_IncludeTeach.Text = "Teachable";
            this.chkEgg_IncludeTeach.UseVisualStyleBackColor = true;
            //
            // btnExportEgg
            //
            this.btnExportEgg.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExportEgg.Location = new System.Drawing.Point(exportX, exportY);
            this.btnExportEgg.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnExportEgg.Name = "btnExportEgg";
            this.btnExportEgg.Size = new System.Drawing.Size(columnWidth, btnHeight);
            this.btnExportEgg.TabIndex = 14;
            this.btnExportEgg.Text = "Export";
            this.btnExportEgg.UseVisualStyleBackColor = true;
            this.btnExportEgg.Click += new System.EventHandler(this.btnExportEgg_Click);
            //
            // cListEggMoves
            //
            this.cListEggMoves.CheckOnClick = true;
            this.cListEggMoves.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cListEggMoves.FormattingEnabled = true;
            this.cListEggMoves.Items.AddRange(new object[] {
            "SWSH",
            "USUM"});
            this.cListEggMoves.Location = new System.Drawing.Point(5, 25);
            this.cListEggMoves.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cListEggMoves.Name = "cListEggMoves";
            this.cListEggMoves.Size = new System.Drawing.Size(columnWidth, listHeight);
            this.cListEggMoves.TabIndex = 12;
            //
            // bwrkExportTM
            //
            this.bwrkExportTM.WorkerReportsProgress = true;
            this.bwrkExportTM.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bwrkExportTM_DoWork);
            this.bwrkExportTM.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            this.bwrkExportTM.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bwrkGroupMovesets_tm_RunWorkerCompleted);
            //
            // bwrkExportEgg
            //
            this.bwrkExportEgg.WorkerReportsProgress = true;
            this.bwrkExportEgg.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bwrkExportEgg_DoWork);
            this.bwrkExportEgg.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            //
            // btnExportAll
            //
            this.btnExportAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExportAll.Location = new System.Drawing.Point(575, labelY + exportY);
            this.btnOpenOutputFolder.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnExportAll.Name = "btnExportAll";
            this.btnExportAll.Size = new System.Drawing.Size(100, btnHeight);
            this.btnExportAll.TabIndex = 14;
            this.btnExportAll.Text = "Export All";
            this.btnExportAll.UseVisualStyleBackColor = true;
            this.btnExportAll.Click += new System.EventHandler(this.btnExportAll_Click);
            //
            // Form1
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(780, 550);
            this.Controls.Add(this.btnOpenInputFolder);
            this.Controls.Add(this.btnLoadFromSerebii);
            this.Controls.Add(this.cmbGeneration);
            this.Controls.Add(this.btnOpenOutputFolder);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.gBoxOptionsTM);
            this.Controls.Add(this.btnExportAll);
            this.Controls.Add(this.lblLoading);
            this.Controls.Add(this.pbar1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Form1";
            this.Text = "PoryMoves 1.x.x";
            this.gBoxOptionsTM.ResumeLayout(false);
            this.gBoxOptionsTM.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnLoadFromSerebii;
        private System.Windows.Forms.ProgressBar pbar1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Label lblLoading;
        private System.Windows.Forms.ComboBox cmbGeneration;
        private System.Windows.Forms.Button btnWriteLvlLearnsets;
        private System.ComponentModel.BackgroundWorker bwrkExportLvl;
        private System.Windows.Forms.CheckedListBox cListTMMoves;
        private System.Windows.Forms.GroupBox gBoxOptionsTM;
        private System.Windows.Forms.CheckBox chkTM_IncludeLvl;
        private System.Windows.Forms.Button btnExportTM;
        private System.Windows.Forms.CheckBox chkTM_IncludeEgg;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckedListBox cListLevelUp;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox chkEgg_IncludeTeach;
        private System.Windows.Forms.Button btnExportEgg;
        private System.Windows.Forms.CheckBox chkEgg_IncludeLvl;
        private System.Windows.Forms.CheckedListBox cListEggMoves;
        private System.ComponentModel.BackgroundWorker bwrkExportTM;
        private System.Windows.Forms.Button btnLvl_All;
        private System.Windows.Forms.Button btnTM_All;
        private System.Windows.Forms.Button btnEgg_All;
        private System.ComponentModel.BackgroundWorker bwrkExportEgg;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox chkVanillaMode;
        private System.Windows.Forms.Button btnOpenOutputFolder;
        private System.Windows.Forms.Button btnOpenInputFolder;
        private System.Windows.Forms.CheckBox chkGeneral_MewExclusiveTutor;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.CheckBox chkLvl_PreEvo;
        private System.Windows.Forms.Button btnExportAll;
        private System.ComponentModel.BackgroundWorker bwrkExportAll;
    }
}

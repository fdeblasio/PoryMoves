using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using hap = HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading;
using moveParser.data;
using static moveParser.data.Move;
using System.Text.RegularExpressions;
using PoryMoves.entity;
using System.Diagnostics;

namespace moveParser
{
    public partial class Form1 : Form
    {
#if DEBUG
        private static string dbpath = "../../db";
#else
        private static string dbpath = "db";
#endif

        private Dictionary<string, Dictionary<string, MonData>> allGensData = new Dictionary<string, Dictionary<string, MonData>>();
        Dictionary<string, MonData> customGenData = new Dictionary<string, MonData>();
        protected Dictionary<string, GenerationData> GenData;
        protected Dictionary<string, Move> MoveData;

        List<string> validForms = ["ALOLAN", "GALARIAN", "HISUIAN", "PALDEAN"];
        List<string> crossEvoStart = ["Espeon", "Leafeon", "Ursaluna", "Dipplin"];
        List<string> crossEvoEnd = ["Umbreon", "Glaceon", "UrsalunaBloodmoon", "Hydrapple"];

        //Console.WriteLine($"Hello World!");

        public Form1()
        {
            InitializeComponent();

            this.Text = "PoryMoves " + typeof(Form1).Assembly.GetName().Version.ToString(3);

            LoadGenerationData();
            if (cmbGeneration.Items.Count > 0)
                cmbGeneration.SelectedIndex = 0;
#if DEBUG
            cmbGeneration.Visible = true;
            btnLoadFromSerebii.Visible = true;
#endif

            MoveData = MovesData.GetMoveDataFromFile(dbpath + "/moveNames.json");
        }

        protected void LoadGenerationData()
        {
            GenData = GenerationsData.GetGenDataFromFile(dbpath + "/generations.json");
#if DEBUG
            if (!Directory.Exists("db"))
                Directory.CreateDirectory("db");
            File.WriteAllText("db/generations.json", JsonConvert.SerializeObject(GenData, Formatting.Indented));
#endif

            cmbGeneration.Items.Clear();
            cListLevelUp.Items.Clear();
            cListTMMoves.Items.Clear();
            cListEggMoves.Items.Clear();
            int count = 0;
            foreach (KeyValuePair<string, GenerationData> item in GenData)
            {
                cmbGeneration.Items.Add(item.Key);
                cListLevelUp.Items.Add(item.Key);
                cListTMMoves.Items.Add(item.Key);
                cListEggMoves.Items.Add(item.Key);
                switch (item.Key)
                {
                    case "FD":
                    case "SV":
                    case "SWSH":
                    case "USUM":
                        cListLevelUp.SetItemChecked(count, true);
                        cListTMMoves.SetItemChecked(count, true);
                        cListEggMoves.SetItemChecked(count, true);
                        break;
                    case "RSE":
                    case "BDSP":
                        cListTMMoves.SetItemChecked(count, true);
                        break;
                }

                Dictionary<string, MonData> gen = PokemonData.GetMonDataFromFile(dbpath + "/gen/" + item.Value.dbFilename + ".json");
                allGensData.Add(item.Key, gen);

                count++;
            }
            //cListLevelUp.SetItemChecked(0, true);
        }
        private string NameToDefineFormat(string oldname)
        {

            oldname = oldname.Replace("&eacute;", "E");
            oldname = oldname.Replace("-o", "_O");
            oldname = oldname.ToUpper();
            oldname = oldname.Replace(" ", "_");
            oldname = oldname.Replace("'", "");
            oldname = oldname.Replace("-", "_");
            oldname = oldname.Replace(".", "");
            oldname = oldname.Replace("&#9792;", "_F");
            oldname = oldname.Replace("&#9794;", "_M");
            oldname = oldname.Replace(":", "");

            return oldname;
        }

        private string NameToVarFormat(string oldname)
        {
            oldname = oldname.Replace("&eacute;", "e");
            oldname = oldname.Replace("-o", "O");
            oldname = oldname.Replace(" ", "_");
            oldname = oldname.Replace("-", "_");
            oldname = oldname.Replace("'", "");
            oldname = oldname.Replace(".", "");
            oldname = oldname.Replace("&#9792;", "F");
            oldname = oldname.Replace("&#9794;", "M");
            oldname = oldname.Replace(":", "");

            string[] str = oldname.Split('_');
            string final = "";
            foreach (string s in str)
                final += s;

            return final;
        }
        bool InList(List<LevelUpMove> list, LevelUpMove element)
        {
            foreach (LevelUpMove entry in list)
                if (entry.Level == element.Level && entry.Move == element.Move)
                    return true;
            return false;
        }

        private void btnLoadFromSerebii_Click(object sender, EventArgs e)
        {
            SetEnableForAllElements(false);
            backgroundWorker1.RunWorkerAsync(cmbGeneration.SelectedItem.ToString());
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            string current = "";
#if !DEBUG
            try
#endif
            {
                List<MonName> nameList = new List<MonName>();
                Dictionary<string, MonData> Database = new Dictionary<string, MonData>();
                string namesFile = dbpath + "/monNames.json";
#if DEBUG
                if (!Directory.Exists("db"))
                    Directory.CreateDirectory("db");
                File.WriteAllText("db/monNames.json", JsonConvert.SerializeObject(nameList, Formatting.Indented));
#endif

                UpdateLoadingMessage("Loading species...");

                nameList = PokemonData.GetMonNamesFromFile(namesFile);

                GenerationData generation = GenData[(string)e.Argument];

                int namecount = 0;

                foreach (MonName monName in nameList)
                {
                    if (!PokemonData.ShouldSkipMon(monName, generation))
                        namecount++;
                }

                Dictionary<string, MonData> existingMonData = PokemonData.GetMonDataFromFile(dbpath + "/gen/" + generation.dbFilename + ".json");
                int i = 0;
                foreach (MonName monName in nameList)
                {
                    current = monName.DefName;
                    //if (i < 31)
                    {
                        MonData mon = null;
                        MonData currentData = null;
                        if (existingMonData != null && existingMonData.ContainsKey(current))
                            currentData = existingMonData[current];

                        mon = PokemonData.DownloadMonData_PokemonDB(monName, generation, MoveData, currentData);
                        /*
                        if (generation.genNumber <= 7)
                            mon = PokemonData.DownloadMonData_Bulbapedia(item, generation, MoveData);
                        //*/

                        if (mon != null)
                        {
                            try
                            {
                                Database.Add(monName.DefName, mon);
                            }
                            catch (ArgumentException ex)
                            {
                                File.AppendAllText("errorLog.txt", "[" + DateTime.Now.ToString() + "] Error adding " + monName.DefName + ": " + ex.Message);
                            }
                            i++;
                        }
                    }

                    backgroundWorker1.ReportProgress(i * 100 / namecount);
                    // Set the text.
                    UpdateLoadingMessage(i.ToString() + "/" + namecount + " loaded. (Skipped " + (nameList.Count - namecount) + ")");
                }
                if (!Directory.Exists(dbpath))
                    Directory.CreateDirectory(dbpath);
                if (!Directory.Exists(dbpath + "/gen"))
                    Directory.CreateDirectory(dbpath + "/gen");

                File.WriteAllText(dbpath + "/gen/" + generation.dbFilename + ".json", JsonConvert.SerializeObject(Database, Formatting.Indented));
#if DEBUG
                if (!Directory.Exists("db"))
                    Directory.CreateDirectory("db");
                File.WriteAllText("db/gen/" + generation.dbFilename + ".json", JsonConvert.SerializeObject(Database, Formatting.Indented));
#endif

                allGensData.Remove(generation.dbFilename.ToUpper());
                allGensData.Add(generation.dbFilename.ToUpper(), PokemonData.GetMonDataFromFile(dbpath + "/gen/" + generation.dbFilename + ".json"));

                UpdateLoadingMessage("Pokémon data loaded.");
            }
#if !DEBUG
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error loading " + current, MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateLoadingMessage("Couldn't load Pokémon data. (" + current + ")");
            }
#endif
            FinishMoveDataLoading();
        }

        private void backgroundWorker1_ProgressChanged(object sender,
            ProgressChangedEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                pbar1.Value = e.ProgressPercentage;
            });
        }

        public void UpdateLoadingMessage(string newMessage)
        {
            this.Invoke((MethodInvoker)delegate
            {
                this.lblLoading.Text = newMessage;
            });
        }

        public void SetEnableForAllElements(bool value)
        {
            SetEnableForAllButtons(value);
            SetEnableForAllCombobox(value);
            SetEnableForAllComboBoxList(value);
            SetEnableForAllCheckbox(value);
        }

        private void SetEnableForAllButtons(bool value)
        {
            this.Invoke((MethodInvoker)delegate
            {
                btnLoadFromSerebii.Enabled = value;

                btnWriteLvlLearnsets.Enabled = value;
                btnLvl_All.Enabled = value;

                btnExportTM.Enabled = value;
                btnTM_All.Enabled = value;

                btnEgg_All.Enabled = value;
                btnExportEgg.Enabled = value;
            });
        }

        private void SetEnableForAllCombobox(bool value)
        {
            this.Invoke((MethodInvoker)delegate
            {
                cmbGeneration.Enabled = value;
            });
        }

        private void SetEnableForAllComboBoxList(bool value)
        {
            this.Invoke((MethodInvoker)delegate
            {
                cListLevelUp.Enabled = value;
                cListTMMoves.Enabled = value;
                cListEggMoves.Enabled = value;
            });
        }

        private void SetEnableForAllCheckbox(bool value)
        {
            this.Invoke((MethodInvoker)delegate
            {
                chkLvl_PreEvo.Enabled = value;

                chkTM_IncludeEgg.Enabled = value;
                chkTM_IncludeLvl.Enabled = value;

                chkEgg_IncludeLvl.Enabled = value;
                chkEgg_IncludeTeach.Enabled = value;

                chkVanillaMode.Enabled = value;
                chkGeneral_MewExclusiveTutor.Enabled = value;
            });
        }

        public void FinishMoveDataLoading()
        {
            SetEnableForAllElements(true);

            this.Invoke((MethodInvoker)delegate
            {
                this.pbar1.Value = 0;
            });
        }


        private void btnWriteLvlLearnsets_Click(object sender, EventArgs e)
        {
            SetEnableForAllElements(false);
            bwrkExportLvl.RunWorkerAsync();
        }
        private void bwrkExportLvl_DoWork(object sender, DoWorkEventArgs e)
        {
            UpdateLoadingMessage("Grouping movesets...");
            string namesFile = dbpath + "/monNames.json";
            List<MonName> nameList = PokemonData.GetMonNamesFromFile(namesFile);
            if (chkVanillaMode.Checked)
            {
                int number = 252;
                for (char c = 'A'; c <= 'Z'; c++)
                {
                    nameList.Add(new MonName(0, "OldUnown", false, "Old", "Species" + number, "SPECIES_OLD_UNOWN_" + c));
                    number++;
                    //do something with letter 
                }
            }

            customGenData.Clear();

            int i = 1;
            int namecount = nameList.Count;
            foreach (MonName name in nameList)
            {
                MonData monToAdd = new MonData();
                monToAdd.LevelMoves = new List<LevelUpMove>();

                List<string> preEvoMoves = new List<string>();
                List<LevelUpMove> evoMoves = new List<LevelUpMove>();
                List<LevelUpMove> lvl1Moves = new List<LevelUpMove>();

                Dictionary<string, int> OtherLvlMoves = new Dictionary<string, int>();

                bool stopReading = false;
                foreach (string item in cListLevelUp.CheckedItems)
                {
                    GenerationData gen = GenData[item];
                    MonData mon = new MonData();
                    try
                    {
                        mon = allGensData[item][name.DefName];
                    }
                    catch (KeyNotFoundException) { }

                    foreach (LevelUpMove move in mon.LevelMoves)
                    {
                        if (move.Level == 0)
                        {
                            if (AddLevelUpMove(evoMoves, lvl1Moves, OtherLvlMoves, move.Move))
                                evoMoves.Add(move);
                        }
                        else if (move.Level == 1)
                        {
                            if (AddLevelUpMove(evoMoves, lvl1Moves, OtherLvlMoves, move.Move))
                                lvl1Moves.Add(move);
                        }
                        else
                        {
                            if (AddLevelUpMove(evoMoves, lvl1Moves, OtherLvlMoves, move.Move))
                                OtherLvlMoves.Add(move.Move, move.Level);
                        }
                    }
                    foreach(string pem in mon.PreEvoMoves)
                    {
                        if (!preEvoMoves.Contains(pem))
                            preEvoMoves.Add(pem);
                    }
                    if (stopReading)
                        break;
                }

                evoMoves = evoMoves.GroupBy(elem => elem.Move).Select(group => group.First()).ToList();
                lvl1Moves = lvl1Moves.GroupBy(elem => elem.Move).Select(group => group.First()).ToList();

                foreach (LevelUpMove move in evoMoves)
                    monToAdd.LevelMoves.Add(move);

                if (chkLvl_PreEvo.Checked)
                {
                    foreach (string move in preEvoMoves)
                    {
                        if (AddLevelUpMove(evoMoves, lvl1Moves, OtherLvlMoves, move))
                            monToAdd.LevelMoves.Add(new LevelUpMove(1, move));
                    }
                }

                foreach (LevelUpMove move in lvl1Moves)
                    monToAdd.LevelMoves.Add(move);

                foreach (KeyValuePair<string, int> item in OtherLvlMoves)
                    monToAdd.LevelMoves.Add(new LevelUpMove(item.Value, item.Key));
                monToAdd.LevelMoves = monToAdd.LevelMoves.OrderBy(o => o.Level).ToList();

                customGenData.Add(name.DefName, monToAdd);

                i++;
                int percent = i * 100 / namecount;
                bwrkExportLvl.ReportProgress(percent);
            }

            if (!Directory.Exists("output"))
                Directory.CreateDirectory("output");

            // file header
            string sets = "#define LEVEL_UP_MOVE(lvl, moveLearned) {.move = moveLearned, .level = lvl}\n";
            sets += "#define LEVEL_UP_END {.move = LEVEL_UP_MOVE_END, .level = 0}\n\nstatic const struct LevelUpMove sNoneLevelUpLearnset[] = {\n    LEVEL_UP_MOVE(1, MOVE_POUND),\n    LEVEL_UP_END\n};\n";

            i = 1;
            string currentFamily = "";
            string currentForm = "";
            // iterate over mons
            foreach (MonName name in nameList)
            {
                MonData mon = new MonData();
                try
                {
                    mon = customGenData[name.DefName];
                    //mon = CustomData[name.DefName];
                }
                catch (KeyNotFoundException) { }

                if (chkVanillaMode.Checked)
                {
                    switch (name.DefName)
                    {
                        case "DEOXYS_NORMAL":
                            name.DefName = "DEOXYS";
                            name.VarName = "Deoxys";
                            break;
                    }
                }

                if (currentFamily != name.FamilyName)
                {
                    if (currentFamily != "")
                        sets += $"#endif //P_FAMILY_{currentFamily}\n";
                    currentFamily = name.FamilyName;
                    sets += $"\n#if P_FAMILY_{currentFamily}";
                }

                if (currentForm != getFormName(name.FormName))
                {
                    currentForm = getFormName(name.FormName);
                    if (validForms.Contains(currentForm))
                        sets += $"\n#if P_{currentForm}_FORMS";
                }

                if (name.CrossEvo != null && !crossEvoEnd.Contains(name.VarName))
                    sets += $"\n#if P_GEN_{name.CrossEvo}_CROSS_EVOS";

                if (name.VarName == "KyuremWhite" || name.VarName == "CalyrexIceRider")
                    sets += "\n#if P_FUSION_FORMS";

                // begin learnset
                if (!name.usesBaseFormLearnset)
                {
                    sets += $"\nstatic const struct LevelUpMove s{name.VarName}LevelUpLearnset[] = {{\n";

                    if (mon.LevelMoves.Count == 0)
                        sets += "    LEVEL_UP_MOVE( 1, MOVE_POUND),\n";

                    foreach (LevelUpMove move in mon.LevelMoves)
                    {
                        sets += $"    LEVEL_UP_MOVE({move.Level,2}, {move.Move}),\n";
                    }
                    sets += "    LEVEL_UP_END\n};\n";
                }
                if (name.VarName == "KyuremBlack" || name.VarName == "CalyrexShadowRider")
                    sets += "#endif //P_FUSION_FORMS\n";
                if (name.CrossEvo != null && !crossEvoStart.Contains(name.VarName) && name.SpeciesName != "Porygon2")
                    sets += $"#endif //P_GEN_{name.CrossEvo}_CROSS_EVOS\n";
                if (name.SpeciesName == "Porygon-Z")
                    sets += $"#endif //P_GEN_2_CROSS_EVOS\n";
                if (name.FormEnd)
                    sets += $"#endif //P_{currentForm}_FORMS\n";
                if (currentFamily == "PECHARUNT")
                    sets += $"#endif //P_FAMILY_{currentFamily}\n";

                int percent = i * 100 / namecount;
                bwrkExportLvl.ReportProgress(percent);
                // Set the text.
                UpdateLoadingMessage(i.ToString() + "/" + namecount + " level up movesets exported.");
                i++;
            }

            // write to file
            File.WriteAllText("output/level_up_learnsets.h", sets);

            bwrkExportLvl.ReportProgress(0);

            MessageBox.Show("Level Up moves exported to \"output/level_up_learnsets.h\"", "Success!", MessageBoxButtons.OK);
            SetEnableForAllElements(true);
        }

        private void bwrkGroupMovesets_tm_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        private void bwrkExportTM_DoWork(object sender, DoWorkEventArgs e)
        {
            UpdateLoadingMessage("Grouping movesets...");
            string namesFile = dbpath + "/monNames.json";
            List<MonName> nameList = PokemonData.GetMonNamesFromFile(namesFile);

            Dictionary<string, List<string>> lvlMoves = new Dictionary<string, List<string>>();

            customGenData.Clear();

            int i = 0;
            int namecount = nameList.Count;
            foreach (MonName name in nameList)
            {
                MonData monToAdd = new MonData();
                monToAdd.TMMoves = new List<string>();
                lvlMoves.Add(name.DefName, new List<string>());

                foreach (string item in cListTMMoves.CheckedItems)
                {
                    GenerationData gen = GenData[item];
                    MonData mon;
                    try
                    {
                        mon = allGensData[item][name.DefName];
                    }
                    catch (KeyNotFoundException)
                    {
                        mon = new MonData();
                    }
                    foreach (string move in mon.TMMoves)
                        monToAdd.TMMoves.Add(move);
                    foreach (string move in mon.TutorMoves)
                        monToAdd.TutorMoves.Add(move);
                    if (chkTM_IncludeLvl.Checked)
                    {
                        foreach (LevelUpMove move in mon.LevelMoves)
                            lvlMoves[name.DefName].Add(move.Move);
                    }
                    if (chkTM_IncludeEgg.Checked)
                    {
                        foreach (string move in mon.EggMoves)
                            monToAdd.EggMoves.Add(move);
                    }
                }
                monToAdd.TMMoves = monToAdd.TMMoves.GroupBy(elem => elem).Select(group => group.First()).ToList();

                customGenData.Add(name.DefName, monToAdd);

                i++;
                int percent = i * 100 / namecount;
                bwrkExportTM.ReportProgress(percent);
            }

            // load specified TM list
            List<string> tmMovesTemp = new List<string>();
            tmMovesTemp = File.ReadAllLines("\\\\wsl.localhost\\Ubuntu\\home\\frank\\pokeemerald-expansion\\include\\constants\\tms_hms.h").ToList();
            List<string> tmMoves = new List<string>();
            string writeText = "";
            foreach (string str in tmMovesTemp)
            {
                writeText += str + "\n";    
                if (str.Trim().StartsWith("F("))
                    tmMoves.Add("MOVE_" + str.Trim().Replace("F(", "").Replace(")", "").Replace(" \\", ""));
            }
            List<string> tutorMovesTemp = new List<string>();
            if (Directory.Exists("input") && File.Exists("input/tutor.txt"))
                tutorMovesTemp = File.ReadAllLines("input/tutor.txt").ToList();
            List<string> tutorMoves = new List<string>();

            foreach (string str in tutorMovesTemp)
            {
                if (!str.Trim().Equals("") && !str.Trim().StartsWith("//"))
                    tutorMoves.Add(str);
            }

            // file header
            string sets = "static const u16 sNoneTeachableLearnset[] = {\n    MOVE_UNAVAILABLE,\n};\n";

            i = 1;
            string currentFamily = "";
            string currentForm = "";
            // iterate over mons
            foreach (MonName name in nameList)
            {
                if (chkVanillaMode.Checked)
                {
                    switch (name.DefName)
                    {
                        case "DEOXYS_NORMAL":
                            name.DefName = "DEOXYS";
                            name.VarName = "Deoxys";
                            break;
                    }
                }
                MonData data = customGenData[name.DefName];

                if (currentFamily != name.FamilyName)
                {
                    if (currentFamily != "")
                        sets += $"#endif //P_FAMILY_{currentFamily}\n";
                    currentFamily = name.FamilyName;
                    sets += $"\n#if P_FAMILY_{currentFamily}";
                }

                if (currentForm != getFormName(name.FormName))
                {
                    currentForm = getFormName(name.FormName);
                    if (validForms.Contains(currentForm))
                        sets += $"\n#if P_{currentForm}_FORMS";
                }

                if (name.CrossEvo != null && !crossEvoEnd.Contains(name.VarName))
                    sets += $"\n#if P_GEN_{name.CrossEvo}_CROSS_EVOS";

                // begin learnset
                if (!name.usesBaseFormLearnset && name.VarName != "KyuremWhite" && name.VarName != "KyuremBlack")
                {
                    if (name.VarName == "CalyrexIceRider")
                        sets += "\n#if P_FUSION_FORMS";

                    List<string> teachableLearnsets = new List<string>();

                    if (name.SpeciesName == "Mew")
                        sets += "\n// Instead of reading this array for Mew, it checks for exceptions in CanLearnTeachableMove instead.";
                    if (name.VarName == "OinkologneMale")
                        sets += $"\nstatic const u16 s{name.SpeciesName}TeachableLearnset[] = {{\n";
                    else if (name.VarName != "OinkologneFemale")
                        sets += $"\nstatic const u16 s{name.VarName}TeachableLearnset[] = {{\n";

                    foreach (string move in lvlMoves[name.DefName])
                        if (AddTeachableMove(teachableLearnsets, tmMoves, tutorMoves, move))
                            teachableLearnsets.Add(move);

                    foreach (string move in data.TMMoves)
                        if (AddTeachableMove(teachableLearnsets, tmMoves, tutorMoves, move))
                            teachableLearnsets.Add(move);

                    foreach (string move in data.EggMoves)
                        if (AddTeachableMove(teachableLearnsets, tmMoves, tutorMoves, move))
                            teachableLearnsets.Add(move);

                    foreach (string move in data.TutorMoves)
                        if (AddTeachableMove(teachableLearnsets, tmMoves, tutorMoves, move))
                            teachableLearnsets.Add(move);

                    // Include universal TM moves
                    foreach (string tmMove in tmMoves)
                    {
                        // Adds TM if it's Mew.
                        if (!teachableLearnsets.Contains(tmMove) && name.NatDexNum == 151)
                            teachableLearnsets.Add(tmMove);
                    }

                    foreach (string tutorMove in tutorMoves)
                    {
                        // Adds Tutor move if it's Mew.
                        if (!teachableLearnsets.Contains(tutorMove) && !FrankDexit(tutorMove) && CanMewLearnMove(name.NatDexNum, tutorMove))
                            teachableLearnsets.Add(tutorMove);
                    }

                    // Order alphabetically
                    teachableLearnsets = teachableLearnsets.OrderBy(x => x).ToList();

                    if (name.VarName != "OinkologneFemale"){
                        foreach (string move in teachableLearnsets)
                        {
                            //Gender-unknown and Nincada's family shouldn't learn Attract.)
                            if (!((name.isGenderless || name.SpeciesName == "Nincada" || name.SpeciesName == "Ninjask") && move.Equals("MOVE_ATTRACT")) && !IsMoveUniversal(move))
                                sets += $"    {move},\n";
                        }
                        sets += "    MOVE_UNAVAILABLE,\n};\n";
                    }
                    if (name.VarName == "CalyrexShadowRider")
                        sets += "#endif //P_FUSION_FORMS\n";
                    if (name.CrossEvo != null && !crossEvoStart.Contains(name.VarName) && name.SpeciesName != "Porygon2")
                        sets += $"#endif //P_GEN_{name.CrossEvo}_CROSS_EVOS\n";
                    if (name.SpeciesName == "Porygon-Z")
                        sets += $"#endif //P_GEN_2_CROSS_EVOS\n";
                    if (name.FormEnd)
                        sets += $"#endif //P_{currentForm}_FORMS\n";
                    if (currentFamily == "PECHARUNT")
                        sets += $"#endif //P_FAMILY_{currentFamily}\n";
                }

                int percent = i * 100 / namecount;
                bwrkExportTM.ReportProgress(percent);
                // Set the text.
                UpdateLoadingMessage(i.ToString() + "/" + namecount + " teachable movesets exported.");
                i++;
            }

            // write to file
            File.WriteAllText("output/teachable_learnsets.h", sets);

            bwrkExportTM.ReportProgress(0);

            MessageBox.Show("Teachable moves exported to \"output/teachable_learnsets.h\"", "Success!", MessageBoxButtons.OK);
            SetEnableForAllElements(true);
        }

        private void btnExportTM_Click(object sender, EventArgs e)
        {
            SetEnableForAllElements(false);
            bwrkExportTM.RunWorkerAsync();
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show("Loading from the Internet complete!", "Success!", MessageBoxButtons.OK);
        }

        private void btnLvl_All_Click(object sender, EventArgs e)
        {
            if (btnLvl_All.Text.Equals("Select All"))
            {
                for (int i = 0; i < cListLevelUp.Items.Count; ++i)
                    cListLevelUp.SetItemChecked(i, true);
                btnLvl_All.Text = "Deselect All";
            }
            else
            {
                for (int i = 0; i < cListLevelUp.Items.Count; ++i)
                    cListLevelUp.SetItemChecked(i, false);
                btnLvl_All.Text = "Select All";
            }
        }

        private void btnTM_All_Click(object sender, EventArgs e)
        {
            if (btnTM_All.Text.Equals("Select All"))
            {
                for (int i = 0; i < cListTMMoves.Items.Count; ++i)
                    cListTMMoves.SetItemChecked(i, true);
                btnTM_All.Text = "Deselect All";
            }
            else
            {
                for (int i = 0; i < cListTMMoves.Items.Count; ++i)
                    cListTMMoves.SetItemChecked(i, false);
                btnTM_All.Text = "Select All";
            }
        }

        private void btnEgg_All_Click(object sender, EventArgs e)
        {
            if (btnEgg_All.Text.Equals("Select All"))
            {
                for (int i = 0; i < cListEggMoves.Items.Count; ++i)
                    cListEggMoves.SetItemChecked(i, true);
                btnEgg_All.Text = "Deselect All";
            }
            else
            {
                for (int i = 0; i < cListEggMoves.Items.Count; ++i)
                    cListEggMoves.SetItemChecked(i, false);
                btnEgg_All.Text = "Select All";
            }
        }

        private void btnExportEgg_Click(object sender, EventArgs e)
        {
            SetEnableForAllElements(false);
            bwrkExportEgg.RunWorkerAsync();
        }

        private void bwrkExportEgg_DoWork(object sender, DoWorkEventArgs e)
        {
            UpdateLoadingMessage("Grouping movesets...");
            string namesFile = dbpath + "/monNames.json";
            List<MonName> nameList = PokemonData.GetMonNamesFromFile(namesFile);

            Dictionary<string, List<string>> lvlMoves = new Dictionary<string, List<string>>();

            customGenData.Clear();

            int i = 0;
            int namecount = nameList.Count;
            foreach (MonName name in nameList)
            {
                MonData monToAdd = new MonData();
                monToAdd.EggMoves = new List<string>();
                lvlMoves.Add(name.DefName, new List<string>());

                foreach (string item in cListEggMoves.CheckedItems)
                {
                    GenerationData gen = GenData[item];
                    MonData mon;
                    try
                    {
                        mon = allGensData[item][name.DefName];
                    }
                    catch (KeyNotFoundException)
                    {
                        mon = new MonData();
                    }
                    foreach (string move in mon.EggMoves)
                        monToAdd.EggMoves.Add(move);
                    if (chkEgg_IncludeLvl.Checked)
                    {
                        foreach (LevelUpMove move in mon.LevelMoves)
                            monToAdd.EggMoves.Add(move.Move);
                    }
                    if (chkEgg_IncludeTeach.Checked)
                    {
                        foreach (string move in mon.TMMoves)
                            monToAdd.EggMoves.Add(move);
                        foreach (string move in mon.TutorMoves)
                            monToAdd.EggMoves.Add(move);
                    }


                }
                monToAdd.EggMoves = monToAdd.EggMoves.GroupBy(elem => elem).Select(group => group.First()).OrderBy(x => x).ToList();

                customGenData.Add(name.DefName, monToAdd);

                i++;
                int percent = i * 100 / namecount;
                bwrkExportEgg.ReportProgress(percent);
            }

            // file header
            string sets = "#include \"constants/moves.h\"\n\n" +
                            "#define EGG_MOVES_SPECIES_OFFSET 20000\n" +
                            "#define EGG_MOVES_TERMINATOR 0xFFFF\n" +
                            "#define egg_moves(species, moves...) (SPECIES_##species + EGG_MOVES_SPECIES_OFFSET), moves\n\n" +
                            "const u16 gEggMoves[] = {\n";

            // iterate over mons
            i = 1;
            string currentFamily = "";
            foreach (MonName name in nameList)
            {
                string currentForm = getFormName(name.FormName);
                if (chkVanillaMode.Checked)
                {
                    switch (name.DefName)
                    {
                        case "DEOXYS_NORMAL":
                            name.DefName = "DEOXYS";
                            name.VarName = "Deoxys";
                            break;
                    }
                }
                MonData data = customGenData[name.DefName];
                if (name.CanHatchFromEgg && data.EggMoves.Count > 0)
                {
                    if (currentFamily != name.FamilyName)
                    {
                        if (currentFamily != "")
                            sets += $"#endif //P_FAMILY_{currentFamily}\n\n";
                        currentFamily = name.FamilyName;
                        sets += $"#if P_FAMILY_{currentFamily}";
                    }

                    if (validForms.Contains(currentForm))
                        sets += $"\n#if P_{currentForm}_FORMS";

                    if (name.CrossEvo != null)
                        sets += $"\n#if P_GEN_{name.CrossEvo}_CROSS_EVOS";

                    // begin learnset
                    sets += $"\n    egg_moves({name.DefName},\n";
                    // hacky workaround for first move being on the same line
                    int eggm = 1;
                    foreach (string move in data.EggMoves)
                    {
                        sets += $"        {move}";
                        if (eggm == data.EggMoves.Count)
                            sets += ")";
                        sets += ",\n";
                        eggm++;
                    }
                    if (name.CrossEvo != null)
                        sets += $"#endif //P_GEN_{name.CrossEvo}_CROSS_EVOS\n";
                    if (validForms.Contains(currentForm))
                        sets += $"#endif //P_{currentForm}_FORMS\n";
                }

                int percent = i * 100 / namecount;
                bwrkExportEgg.ReportProgress(percent);
                // Set the text.
                UpdateLoadingMessage(i.ToString() + "/" + namecount + " Egg movesets exported.");
                i++;
            }

            sets += "    EGG_MOVES_TERMINATOR\n};\n";

            // write to file
            File.WriteAllText("output/egg_moves.h", sets);

            bwrkExportEgg.ReportProgress(0);

            MessageBox.Show("Egg moves exported to \"output/egg_moves.h\"", "Success!", MessageBoxButtons.OK);
            SetEnableForAllElements(true);
        }

        private void btnOpenInputFolder_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists("input"))
                Directory.CreateDirectory("input");

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                Arguments = "input",
                FileName = "explorer.exe"
            };
            Process.Start(startInfo);
        }

        private void btnOpenOutputFolder_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists("output"))
                Directory.CreateDirectory("output");

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                Arguments = "output",
                FileName = "explorer.exe"
            };
            Process.Start(startInfo);
        }

        private void cmbTM_ExportMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            chkTM_IncludeEgg.Checked = true;
            chkTM_IncludeLvl.Checked = true;
        }

        private bool CanMewLearnMove(int natDexNum, string move)
        {
            if (natDexNum != 151)
                return false;
            if (!chkGeneral_MewExclusiveTutor.Checked)
            {
                switch(move)
                {
                    case "MOVE_RELIC_SONG":
                    case "MOVE_SECRET_SWORD":
                    case "MOVE_DRAGON_ASCENT":
                    case "MOVE_VOLT_TACKLE":
                    case "MOVE_ZIPPY_ZAP":
                    case "MOVE_FLOATY_FALL":
                    case "MOVE_SPLISHY_SPLASH":
                    case "MOVE_BOUNCY_BUBBLE":
                    case "MOVE_BUZZY_BUZZ":
                    case "MOVE_SIZZLY_SLIDE":
                    case "MOVE_GLITZY_GLOW":
                    case "MOVE_BADDY_BAD":
                    case "MOVE_SAPPY_SEED":
                    case "MOVE_FREEZY_FROST":
                    case "MOVE_SPARKLY_SWIRL":
                        return false;
                }
            }
            return true;
        }

        private bool IsMoveUniversal(string move)
        {
            switch(move)
            {
                case "MOVE_BIDE":
                case "MOVE_FRUSTRATION":
                case "MOVE_HIDDEN_POWER":
                case "MOVE_MIMIC":
                case "MOVE_NATURAL_GIFT":
                case "MOVE_RAGE":
                case "MOVE_RETURN":
                case "MOVE_SECRET_POWER":
                case "MOVE_SUBSTITUTE":
                case "MOVE_TERA_BLAST":
                    return true;
                default:
                    return false;
            }
        }

        private bool FrankDexit(string move)
        {
            switch(move)
            {
                case "MOVE_LUCKY_CHANT":
                case "MOVE_NATURAL_GIFT":
                case "MOVE_SYNCHRONOISE":
                case "MOVE_WRING_OUT":
                    return true;
                default:
                    return false;
            }
        }

        private bool AddLevelUpMove(List<LevelUpMove> evoMoves, List<LevelUpMove> lvl1Moves, Dictionary<string, int> OtherLvlMoves, string move)
        {
            if(!evoMoves.Select(x => x.Move).Contains(move)
            && !lvl1Moves.Select(x => x.Move).Contains(move)
            && !OtherLvlMoves.ContainsKey(move)
            && !FrankDexit(move))
                return true;
            else
                return false;
        }

        private bool AddTeachableMove(List<string> teachableLearnsets, List<string> tmMoves, List<string> tutorMoves, string move)
        {
            if (!teachableLearnsets.Contains(move)  && (tmMoves.Contains(move) || tutorMoves.Contains(move)) && !FrankDexit(move))
                return true;
            else
                return false;
        }

        private string getFormName(string formName)
        {
            switch (formName)
            {
                case "Perrserker":
                case "Sirfetch'd":
                case "Mr. Rime":
                case "Cursola":
                case "Obstagoon":
                case "Runerigus":
                    return "GALARIAN";
                case "Overqwil":
                case "Sneasler":
                case "White-Striped Form":
                case "Basculegion":
                    return "HISUIAN";
                case "Clodsire":
                    return "PALDEAN";
                default:
                    return formName.Split()[0].ToUpper();
            }
        }
    }
}

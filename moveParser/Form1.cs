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

        enum MoveCombination
        {
            LatestPlus,
            UseLatest,
            Combine,
            CombineMax,
        }

        public Form1()
        {
            InitializeComponent();

            this.Text = "PoryMoves " + typeof(Form1).Assembly.GetName().Version.ToString(3);

            LoadGenerationData();
            if (cmbGeneration.Items.Count > 0)
                cmbGeneration.SelectedIndex = 0;
            LoadExportModes();
#if DEBUG
            cmbGeneration.Visible = true;
            btnLoadFromSerebii.Visible = true;
#endif

            MoveData = MovesData.GetMoveDataFromFile(dbpath + "/moveNames.json");
        }

        protected void LoadExportModes()
        {
            cmbLvl_Combine.Items.Insert((int)MoveCombination.LatestPlus, "Latest + Removed");
            cmbLvl_Combine.Items.Insert((int)MoveCombination.UseLatest, "Use Latest Moveset");
            cmbLvl_Combine.Items.Insert((int)MoveCombination.Combine, "Combine Movesets (Avg)");
            cmbLvl_Combine.Items.Insert((int)MoveCombination.CombineMax, "Combine Movesets (Max)");
            cmbLvl_Combine.SelectedIndex = 0;
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
            cListTutorMoves.Items.Clear();
            int count = 0;
            foreach (KeyValuePair<string, GenerationData> item in GenData)
            {
                cmbGeneration.Items.Add(item.Key);
                cListLevelUp.Items.Add(item.Key);
                cListTMMoves.Items.Add(item.Key);
                cListEggMoves.Items.Add(item.Key);
                cListTutorMoves.Items.Add(item.Key);
                switch (item.Value.lvlUpColumn)
                {
                    case "SV":
                    case "SwSh":
                    case "USUM":
                        cListLevelUp.SetItemChecked(count, true);
                        cListTMMoves.SetItemChecked(count, true);
                        cListEggMoves.SetItemChecked(count, true);
                        cListTutorMoves.SetItemChecked(count, true);
                        break;
                    case "RSE":
                        cListTMMoves.SetItemChecked(count, true);
                        cListEggMoves.SetItemChecked(count, true);
                        cListTutorMoves.SetItemChecked(count, true);
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
                        if (generation.genNumber > 7)
                            mon = PokemonData.DownloadMonData_Serebii(item, generation, MoveData);
                        else
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
                    UpdateLoadingMessage(i.ToString() + " out of " + namecount + " loaded. (Skipped " + (nameList.Count - namecount) + ")");
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

                btnExportTutor.Enabled = value;
                btnTutor_All.Enabled = value;
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
                cListTutorMoves.Enabled = value;
                cListEggMoves.Enabled = value;
            });
        }

        private void SetEnableForAllCheckbox(bool value)
        {
            this.Invoke((MethodInvoker)delegate
            {
                chkLvl_PreEvo.Enabled = value;
                cmbLvl_Combine.Enabled = value;

                chkTM_IncludeEgg.Enabled = value;
                chkTM_IncludeLvl.Enabled = value;
                chkTM_IncludeTutor.Enabled = value;

                chkTutor_IncludeLvl.Enabled = value;
                chkTutor_IncludeEgg.Enabled = value;
                chkTutor_IncludeTM.Enabled = value;

                chkEgg_IncludeLvl.Enabled = value;
                chkEgg_IncludeTM.Enabled = value;
                chkEgg_IncludeTutor.Enabled = value;

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
            MoveCombination mode = MoveCombination.UseLatest;
            this.Invoke((MethodInvoker)delegate
            {
                mode = (MoveCombination)this.cmbLvl_Combine.SelectedIndex;
            });

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

                Dictionary<string, List<Tuple<int, int>>> OtherLvlMoves = new Dictionary<string, List<Tuple<int, int>>>();

                bool stopReading = false;
                foreach (string item in cListLevelUp.CheckedItems)
                {
                    GenerationData gen = GenData[item];
                    MonData mon = new MonData();
                    try
                    {
                        mon = allGensData[item][name.DefName];
                        if (mode == MoveCombination.UseLatest
                            && allGensData[item][name.DefName].TotalMoveCount() != 0
                            && gen.gameId != 19) // Exclude PLA movesets from "Latest Moveset Only" config
                        {
                            stopReading = true;
                        }
                    }
                    catch (KeyNotFoundException)
                    {
                    }

                    foreach (LevelUpMove move in mon.LevelMoves)
                    {
                        if (move.Level == 0)
                        {
                            if (AddMove(evoMoves, lvl1Moves, OtherLvlMoves, move.Move))
                                evoMoves.Add(move);
                        }
                        else if (move.Level == 1)
                        {
                            if (AddMove(evoMoves, lvl1Moves, OtherLvlMoves, move.Move))
                                lvl1Moves.Add(move);
                        }
                        else
                        {
                            if (mode == MoveCombination.LatestPlus)
                            {
                                if (AddMove(evoMoves, lvl1Moves, OtherLvlMoves, move.Move))
                                    OtherLvlMoves.Add(move.Move, new List<Tuple<int, int>> { new Tuple<int, int>(gen.genNumber, move.Level) });
                            }
                            else if (!OtherLvlMoves.ContainsKey(move.Move))
                                OtherLvlMoves.Add(move.Move, new List<Tuple<int, int>> { new Tuple<int, int>(gen.genNumber, move.Level) });
                            else
                                OtherLvlMoves[move.Move].Add(new Tuple<int, int>(gen.genNumber, move.Level));
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
                        if (AddMove(evoMoves, lvl1Moves, OtherLvlMoves, move))
                            monToAdd.LevelMoves.Add(new LevelUpMove(1, move));
                    }
                }

                foreach (LevelUpMove move in lvl1Moves)
                    monToAdd.LevelMoves.Add(move);

                //monToAdd.LevelMoves.Add(new LevelUpMove(21, "MOVE_SKETCH"));
                if (!name.SpeciesName.Equals("Smeargle"))
                {
                    foreach (KeyValuePair<string, List<Tuple<int, int>>> item in OtherLvlMoves)
                    {
                        if (mode == MoveCombination.CombineMax)
                        {
                            int max = 0;

                            foreach (Tuple<int, int> l in item.Value)
                            {
                                max = Math.Max(max, l.Item2);
                            }
                            monToAdd.LevelMoves.Add(new LevelUpMove(Math.Max(max, 2), item.Key));
                        }
                        else
                        {

                            int weightedSum = 0;
                            int sum = 0;

                            foreach (Tuple<int, int> l in item.Value)
                            {
                                weightedSum += l.Item1 * l.Item2;
                                sum += l.Item1;
                            }
                            monToAdd.LevelMoves.Add(new LevelUpMove(Math.Max((int)(weightedSum / sum), 2), item.Key));
                        }
                    }
                }
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

            // iterate over mons
            i = 1;
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

                int percent = i * 100 / namecount;
                bwrkExportLvl.ReportProgress(percent);
                // Set the text.
                UpdateLoadingMessage(i.ToString() + " out of " + namecount + " Level Up movesets exported.");
                i++;
            }

            sets = replaceOldDefines(sets);

            // write to file
            File.WriteAllText("output/level_up_learnsets.h", sets);

            bwrkExportLvl.ReportProgress(0);
            // Set the text.
            UpdateLoadingMessage(namecount + " Level Up movesets exported.");

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
                    if (chkTM_IncludeTutor.Checked)
                    {
                        foreach (string move in mon.TutorMoves)
                            monToAdd.TutorMoves.Add(move);
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
            if (Directory.Exists("input") && File.Exists("input/tm.txt"))
                tmMovesTemp = File.ReadAllLines("input/tm.txt").ToList();
            List<string> tmMoves = new List<string>();
            string writeText = "";
            foreach (string str in tmMovesTemp)
            {
                writeText += str + "\n";    
                if (!str.Trim().Equals("") && !str.Trim().StartsWith("//"))
                    tmMoves.Add(str);
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
#if DEBUG
            File.WriteAllText("../../input/tm.txt", writeText);
#endif

            // build importable TM list
            string tms = "#ifndef GUARD_CONSTANTS_TMS_HMS_H\n#define GUARD_CONSTANTS_TMS_HMS_H\n\n#define FOREACH_TM(F)";
            foreach (string move in tmMoves)
            {
                tms += $" \\\n    F({move})";
            }
            //seperate out HMs
            tms += "\n\n#define FOREACH_HM(F) \\\n\n#define FOREACH_TMHM(F) \\\n    FOREACH_TM(F) \\\n    FOREACH_HM(F)\n\n#endif\n";

            if (!Directory.Exists("output"))
                Directory.CreateDirectory("output");

            File.WriteAllText("output/tms_hms.h", tms);
            MessageBox.Show("TM list exported to \"output/tms_hms.h\"", "Success!", MessageBoxButtons.OK);

            // file header
            string sets = "static const u16 sNoneTeachableLearnset[] = {\n    MOVE_UNAVAILABLE,\n};\n";

            i = 1;
            // iterate over mons
            foreach (MonName entry in nameList)
            {
                if (chkVanillaMode.Checked)
                {
                    switch (entry.DefName)
                    {
                        case "DEOXYS_NORMAL":
                            entry.DefName = "DEOXYS";
                            entry.VarName = "Deoxys";
                            break;
                    }
                }
                MonData data = customGenData[entry.DefName];
                // begin learnset
                if (!entry.usesBaseFormLearnset)
                {
                    List<string> teachableLearnsets = new List<string>();

                    sets += $"\nstatic const u16 s{entry.VarName}TeachableLearnset[] = {{\n";

                    foreach (string move in lvlMoves[entry.DefName])
                        if (!teachableLearnsets.Contains(move) && !FrankDexit(move))
                            teachableLearnsets.Add(move);

                    foreach (string move in data.TMMoves)
                        if (!teachableLearnsets.Contains(move) && !FrankDexit(move))
                            teachableLearnsets.Add(move);

                    foreach (string move in data.EggMoves)
                        if (!teachableLearnsets.Contains(move) && !FrankDexit(move))
                            teachableLearnsets.Add(move);

                    foreach (string move in data.TutorMoves)
                        if (!teachableLearnsets.Contains(move) && !FrankDexit(move))
                            teachableLearnsets.Add(move);

                    // Include universal TM moves
                    foreach (string tmMove in tmMoves)
                    {
                        string move = "MOVE_" + tmMove;

                        // Adds TM if it's Mew.
                        if (!teachableLearnsets.Contains(move) && entry.NatDexNum == 151)
                            teachableLearnsets.Add(move);
                    }

                    if (chkTM_IncludeTutor.Checked)
                    {
                        foreach (string tutorMove in tutorMoves)
                        {
                            // Adds Tutor move if it's Mew.
                            if (!teachableLearnsets.Contains(tutorMove) && !FrankDexit(tutorMove) && CanMewLearnMove(entry.NatDexNum, tutorMove))
                                teachableLearnsets.Add(tutorMove);
                        }
                    }

                    // Order alphabetically
                    teachableLearnsets = teachableLearnsets.OrderBy(x => x).ToList();

                    foreach (string move in teachableLearnsets)
                    {
                        //Gender-unknown and Nincada's family shouldn't learn Attract.)
                        if (!((entry.isGenderless || entry.NatDexNum == 290 || entry.NatDexNum == 291) && move.Equals("MOVE_ATTRACT")) && !IsMoveUniversal(move))
                            sets += $"    {move},\n";
                    }
                    sets += "    MOVE_UNAVAILABLE,\n};\n";
                }

                int percent = i * 100 / namecount;
                bwrkExportTM.ReportProgress(percent);
                // Set the text.
                UpdateLoadingMessage(i.ToString() + " out of " + namecount + " TM movesets exported.");
                i++;
            }

            sets = replaceOldDefines(sets);

            // write to file
            File.WriteAllText("output/teachable_learnsets.h", sets);

            bwrkExportTM.ReportProgress(0);
            // Set the text.
            UpdateLoadingMessage(namecount + " TM movesets exported.");

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

        private void btnTutor_All_Click(object sender, EventArgs e)
        {
            if (btnTutor_All.Text.Equals("Select All"))
            {
                for (int i = 0; i < cListTutorMoves.Items.Count; ++i)
                    cListTutorMoves.SetItemChecked(i, true);
                btnTutor_All.Text = "Deselect All";
            }
            else
            {
                for (int i = 0; i < cListTutorMoves.Items.Count; ++i)
                    cListTutorMoves.SetItemChecked(i, false);
                btnTutor_All.Text = "Select All";
            }
        }

        private void bwrkExportTutor_DoWork(object sender, DoWorkEventArgs e)
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
                monToAdd.TutorMoves = new List<string>();
                lvlMoves.Add(name.DefName, new List<string>());

                foreach (string item in cListTutorMoves.CheckedItems)
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
                    foreach (string move in mon.TutorMoves)
                        monToAdd.TutorMoves.Add(move);
                    if (chkTutor_IncludeLvl.Checked)
                    {
                        foreach (LevelUpMove move in mon.LevelMoves)
                            lvlMoves[name.DefName].Add(move.Move);
                    }
                    if (chkTutor_IncludeEgg.Checked)
                    {
                        foreach (string move in mon.EggMoves)
                            monToAdd.EggMoves.Add(move);
                    }
                    if (chkTutor_IncludeTM.Checked)
                    {
                        foreach (string move in mon.TMMoves)
                            monToAdd.TMMoves.Add(move);
                    }


                }
                monToAdd.TutorMoves = monToAdd.TutorMoves.GroupBy(elem => elem).Select(group => group.First()).ToList();

                customGenData.Add(name.DefName, monToAdd);

                i++;
                int percent = i * 100 / namecount;
                bwrkExportTutor.ReportProgress(percent);
            }

            // load specified tutor list
            List<string> tutorMovesTemp = new List<string>();
            if (Directory.Exists("input") && File.Exists("input/tutor.txt"))
                tutorMovesTemp = File.ReadAllLines("input/tutor.txt").ToList();
            List<string> tutorMoves = new List<string>();
            
            string writeText = "";
            foreach (string str in tutorMovesTemp)
            {
                writeText += str + "\n";
                if (!str.Trim().Equals("") && !str.Trim().StartsWith("//"))
                    tutorMoves.Add(str);
            }

            List<string> tmMovesTemp = new List<string>();
            if (Directory.Exists("input") && File.Exists("input/tm.txt"))
                tmMovesTemp = File.ReadAllLines("input/tm.txt").ToList();
            List<string> tmMoves = new List<string>();

            foreach (string str in tmMovesTemp)
            {
                if (!str.Trim().Equals("") && !str.Trim().StartsWith("//"))
                    tmMoves.Add(str);
            }
#if DEBUG
            File.WriteAllText("../../input/tutor.txt", writeText);
#endif

            // build importable tutor list
            string tutors = "// IMPORTANT: DO NOT PASTE THIS FILE INTO YOUR REPO!\n// Instead, paste the following list of defines into include/constants/party_menu.h\n\n";
            for (int j = 0; j < tutorMoves.Count; j++)
            {
                string move = tutorMoves[j];
                tutors += $"#define TUTOR_{move,-22} {j,3}\n";
            }
            tutors += $"#define TUTOR_MOVE_COUNT             {tutorMoves.Count,3}";
            File.WriteAllText("output/party_menu_tutor_list.h", tutors);

            string sets = "static const u16 sNoneTeachableLearnset[] = {\n    MOVE_UNAVAILABLE,\n};\n";
            // iterate over mons
            i = 1;

            foreach (MonName entry in nameList)
            {
                if (chkVanillaMode.Checked)
                {
                    switch (entry.DefName)
                    {
                        case "DEOXYS_NORMAL":
                            entry.DefName = "DEOXYS";
                            entry.VarName = "Deoxys";
                            break;
                    }
                }
                MonData data = customGenData[entry.DefName];
                // begin learnset
                if (!entry.usesBaseFormLearnset)
                {
                    List<string> teachableLearnsets = new List<string>();

                    sets += $"\nstatic const u16 s{entry.VarName}TeachableLearnset[] = {{\n";

                    foreach (string move in lvlMoves[entry.DefName])
                        if (!teachableLearnsets.Contains(move) && !FrankDexit(move))
                            teachableLearnsets.Add(move);

                    foreach (string move in data.TMMoves)
                        if (!teachableLearnsets.Contains(move) && !FrankDexit(move))
                            teachableLearnsets.Add(move);

                    foreach (string move in data.EggMoves)
                        if (!teachableLearnsets.Contains(move) && !FrankDexit(move))
                            teachableLearnsets.Add(move);

                    foreach (string move in data.TutorMoves)
                        if (!teachableLearnsets.Contains(move) && !FrankDexit(move))
                            teachableLearnsets.Add(move);

                    foreach (string tutorMove in tutorMoves)
                    {
                        // Adds Tutor move if it's Mew.
                        if (!teachableLearnsets.Contains(tutorMove) && !FrankDexit(tutorMove) && CanMewLearnMove(entry.NatDexNum, tutorMove))
                            teachableLearnsets.Add(tutorMove);
                    }

                    if (chkTutor_IncludeTM.Checked)
                    {
                        // Include universal TM moves
                        foreach (string tmMove in tmMoves)
                        {
                            string move = "MOVE_" + tmMove;

                            // Adds TM if it's Mew
                            if (!teachableLearnsets.Contains(move) && CanMewLearnMove(entry.NatDexNum, move))
                                teachableLearnsets.Add(move);
                        }
                    }

                    // Order alphabetically
                    teachableLearnsets = teachableLearnsets.OrderBy(x => x).ToList();

                    foreach (string move in teachableLearnsets)
                    {
                        //Gender-unknown and Nincada's family shouldn't learn Attract.)
                        if (!((entry.isGenderless || entry.NatDexNum == 290 || entry.NatDexNum == 291) && move.Equals("MOVE_ATTRACT")) && !IsMoveUniversal(move))
                            sets += $"    {move},\n";
                    }
                    sets += "    MOVE_UNAVAILABLE,\n};\n";
                }

                int percent = i * 100 / namecount;
                bwrkExportTutor.ReportProgress(percent);
                // Set the text.
                UpdateLoadingMessage(i.ToString() + " out of " + namecount + " Tutor movesets exported.");
                i++;
            }

            sets = replaceOldDefines(sets);

            // write to file
            File.WriteAllText("output/teachable_learnsets.h", sets);

            bwrkExportTutor.ReportProgress(0);
            // Set the text.
            UpdateLoadingMessage(namecount + " Tutor movesets exported.");

            MessageBox.Show("Teachable moves exported to \"output/teachable_learnsets.h\"", "Success!", MessageBoxButtons.OK);

            SetEnableForAllElements(true);
        }

        private void btnExportTutor_Click(object sender, EventArgs e)
        {
            SetEnableForAllElements(false);
            bwrkExportTutor.RunWorkerAsync();
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
                    if (chkEgg_IncludeTutor.Checked)
                    {
                        foreach (string move in mon.TutorMoves)
                            monToAdd.EggMoves.Add(move);
                    }
                    if (chkEgg_IncludeTM.Checked)
                    {
                        foreach (string move in mon.TMMoves)
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
            foreach (MonName entry in nameList)
            {
                if (chkVanillaMode.Checked)
                {
                    switch (entry.DefName)
                    {
                        case "DEOXYS_NORMAL":
                            entry.DefName = "DEOXYS";
                            entry.VarName = "Deoxys";
                            break;
                    }
                }
                MonData data = customGenData[entry.DefName];
                if (entry.CanHatchFromEgg && data.EggMoves.Count > 0)
                {
                    // begin learnset
                    sets += $"#if P_FAMILY_{entry.DefName}\n    egg_moves({entry.DefName},\n";
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
                    sets += $"#endif //P_FAMILY_{entry.DefName}\n\n";
                }

                int percent = i * 100 / namecount;
                bwrkExportEgg.ReportProgress(percent);
                // Set the text.
                UpdateLoadingMessage(i.ToString() + " out of " + namecount + " Egg movesets exported.");
                i++;
            }

            sets += "    EGG_MOVES_TERMINATOR\n};\n";
            sets = replaceOldDefines(sets);

            // write to file
            File.WriteAllText("output/egg_moves.h", sets);

            bwrkExportEgg.ReportProgress(0);
            // Set the text.
            UpdateLoadingMessage(namecount + " Egg movesets exported.");

            MessageBox.Show("Egg moves exported to \"output/egg_moves.h\"", "Success!", MessageBoxButtons.OK);
            SetEnableForAllElements(true);
        }

        private string replaceOldDefines(string text)
        {
            text = text
                .Replace("MOVE_FAINT_ATTACK", "MOVE_FEINT_ATTACK")
                .Replace("MOVE_SMELLING_SALT", "MOVE_SMELLING_SALTS")
                .Replace("MOVE_VICE_GRIP", "MOVE_VISE_GRIP")
                .Replace("MOVE_HI_JUMP_KICK", "MOVE_HIGH_JUMP_KICK")
                ;

            return text;
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
            chkTM_IncludeTutor.Checked = true;
        }

        private void cmbTutor_ExportMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            chkTutor_IncludeEgg.Checked = true;
            chkTutor_IncludeLvl.Checked = true;
            chkTutor_IncludeTM.Checked = true;
        }

        private bool CanMewLearnMove(int natDexNum, string move)
        {
            if (natDexNum != 151)
                return false;
            if (!chkGeneral_MewExclusiveTutor.Checked)
            {
                switch(move)
                {
                    case "MOVE_DRACO_METEOR":
                    case "MOVE_GRASS_PLEDGE":
                    case "MOVE_FIRE_PLEDGE":
                    case "MOVE_WATER_PLEDGE":
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
                    case "MOVE_STEEL_BEAM":
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

        private bool AddMove(List<LevelUpMove> evoMoves, List<LevelUpMove> lvl1Moves, Dictionary<string, List<Tuple<int, int>>> OtherLvlMoves, string move)
        {
            if(!evoMoves.Select(x => x.Move).Contains(move) 
            && !lvl1Moves.Select(x => x.Move).Contains(move)
            && !OtherLvlMoves.ContainsKey(move)
            && !FrankDexit(move))
                return true;
            else
                return false;
        }

        private void cmbLvl_Combine_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}

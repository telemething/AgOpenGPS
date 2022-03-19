﻿using System;
using System.Text;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormConfig
    {

        #region Module Steer
        private void tabASteer_Enter(object sender, EventArgs e)
        {

            if (!Properties.Settings.Default.setGPS_isGPSTool 
                && !Properties.Settings.Default.setGPS_isGPSToolOnly)
            {
                cboxGPSNormal.Checked = true;
                cboxGPSToolOnOff.Checked = false;
                cboxGPSToolOnlyOnOff.Checked = false;
            }
            else
            {
                cboxGPSNormal.Checked = false;
            }

            cboxGPSToolOnOff.Checked = Properties.Settings.Default.setGPS_isGPSTool;
            cboxGPSToolOnlyOnOff.Checked = Properties.Settings.Default.setGPS_isGPSToolOnly;

            hsbarTool_P.Value = Properties.Vehicle.Default.setTool_P;
            hsbarTool_I.Value = Properties.Vehicle.Default.setTool_I;
            hsbarTool_MinPWM.Value = Properties.Vehicle.Default.setTool_MinPWM;
            hsbarTool_LowPWM.Value = Properties.Vehicle.Default.setTool_LowPWM;
            hsbarTool_HighPWM.Value = Properties.Vehicle.Default.setTool_HighPWM;
            hsbarTool_Counts.Value = Properties.Vehicle.Default.setTool_Counts;
            hsbarTool_Offset.Value = Properties.Vehicle.Default.setTool_Offset;

            lblTool_P.Text = hsbarTool_P.Value.ToString();
            lblTool_I.Text = hsbarTool_I.Value.ToString();
            lblTool_MinPWM.Text = hsbarTool_MinPWM.Value.ToString();
            lblTool_LowPWM.Text = hsbarTool_LowPWM.Value.ToString();
            lblTool_HighPWM.Text = hsbarTool_HighPWM.Value.ToString();
            lblTool_Counts.Text = hsbarTool_Counts.Value.ToString();
            lblTool_Offset.Text = (hsbarTool_Offset.Value - 127).ToString();

            pboxSendToolSteer.Visible = false;

            if (cboxGPSToolOnlyOnOff.Checked) btnConvertToToolOnly.Visible = true;
            else btnConvertToToolOnly.Visible = false;

        }
        private void btnSendToolSteer_Click(object sender, EventArgs e)
        {
            Properties.Vehicle.Default.setTool_P = (byte)(hsbarTool_P.Value);
            Properties.Vehicle.Default.setTool_I = (byte)(hsbarTool_I.Value);
            Properties.Vehicle.Default.setTool_MinPWM = (byte)(hsbarTool_MinPWM.Value);
            Properties.Vehicle.Default.setTool_LowPWM = (byte)(hsbarTool_LowPWM.Value);
            Properties.Vehicle.Default.setTool_HighPWM = (byte)(hsbarTool_HighPWM.Value);
            Properties.Vehicle.Default.setTool_Counts = (byte)(hsbarTool_Counts.Value);
            Properties.Vehicle.Default.setTool_Offset = (byte)(hsbarTool_Offset.Value);

            mf.p_233.pgn[mf.p_233.P] = (byte)(hsbarTool_P.Value);
            mf.p_233.pgn[mf.p_233.I] = (byte)(hsbarTool_I.Value);
            mf.p_233.pgn[mf.p_233.minPWM] = (byte)(hsbarTool_MinPWM.Value);
            mf.p_233.pgn[mf.p_233.lowPWM] = (byte)(hsbarTool_LowPWM.Value);
            mf.p_233.pgn[mf.p_233.highPWM] = (byte)(hsbarTool_HighPWM.Value);
            mf.p_233.pgn[mf.p_233.counts] = (byte)(hsbarTool_Counts.Value);
            mf.p_233.pgn[mf.p_233.offset] = (byte)(hsbarTool_Offset.Value);

            mf.SendPgnToLoop(mf.p_233.pgn);
            pboxSendToolSteer.Visible = false;
        }

        private void hsbarTool_Counts_ValueChanged(object sender, EventArgs e)
        {
            pboxSendToolSteer.Visible = true;
            lblTool_Counts.Text = hsbarTool_Counts.Value.ToString();
        }

        private void hsbarTool_P_ValueChanged(object sender, EventArgs e)
        {
            pboxSendToolSteer.Visible = true;
            lblTool_P.Text = hsbarTool_P.Value.ToString();
        }

        private void hsbarTool_I_ValueChanged(object sender, EventArgs e)
        {
            pboxSendToolSteer.Visible = true;
            lblTool_I.Text = hsbarTool_I.Value.ToString();
        }

        private void hsbarTool_MinPWM_ValueChanged(object sender, EventArgs e)
        {
            pboxSendToolSteer.Visible = true;
            lblTool_MinPWM.Text = hsbarTool_MinPWM.Value.ToString();
        }

        private void hsbarTool_LowPWM_ValueChanged(object sender, EventArgs e)
        {
            pboxSendToolSteer.Visible = true;
            lblTool_LowPWM.Text = hsbarTool_LowPWM.Value.ToString();
        }

        private void hsbarTool_HighPWM_ValueChanged(object sender, EventArgs e)
        {
            pboxSendToolSteer.Visible = true;
            lblTool_HighPWM.Text = hsbarTool_HighPWM.Value.ToString();
        }

        private void hsbarTool_Offset_ValueChanged(object sender, EventArgs e)
        {
            pboxSendToolSteer.Visible = true;
            lblTool_Offset.Text = (hsbarTool_Offset.Value - 127).ToString();
        }

        private void cboxGPSToolOnOff_Click(object sender, EventArgs e)
        {
            if (cboxGPSToolOnOff.Checked) return;
            else cboxGPSToolOnOff.Checked = true;
                if (cboxGPSToolOnlyOnOff.Checked) cboxGPSToolOnlyOnOff.Checked = false;
            if (cboxGPSNormal.Checked) cboxGPSNormal.Checked = false;
            if (cboxGPSToolOnlyOnOff.Checked) btnConvertToToolOnly.Visible = true;
            else btnConvertToToolOnly.Visible = false;

        }
        private void cboxGPSToolOnlyOnOff_Click(object sender, EventArgs e)
        {
            if (cboxGPSToolOnlyOnOff.Checked) return;
            else cboxGPSToolOnlyOnOff.Checked = true;
                if (cboxGPSToolOnOff.Checked) cboxGPSToolOnOff.Checked = false;
            if (cboxGPSNormal.Checked) cboxGPSNormal.Checked = false;
            if (cboxGPSToolOnlyOnOff.Checked) btnConvertToToolOnly.Visible = true;
            else btnConvertToToolOnly.Visible = false;
        }

        private void cboxGPSNormal_Click(object sender, EventArgs e)
        {
            if (cboxGPSNormal.Checked) return;
            else cboxGPSNormal.Checked = true;
                if (cboxGPSToolOnlyOnOff.Checked) cboxGPSToolOnlyOnOff.Checked = false;
            if (cboxGPSToolOnOff.Checked) cboxGPSToolOnOff.Checked = false;
            if (cboxGPSToolOnlyOnOff.Checked) btnConvertToToolOnly.Visible = true;
            else btnConvertToToolOnly.Visible = false;
        }

        private void btnConvertToToolOnly_Click(object sender, EventArgs e)
        {
            DialogResult result3 = MessageBox.Show("Are you sure you want to change settings to Tool Only?",
                "Tool Only Settings",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);

            if (result3 == DialogResult.Yes)
            {
                //make it see thru
                Properties.Settings.Default.setDisplay_vehicleOpacity = 23;
                mf.vehicleOpacity = 0.23;
                mf.vehicleOpacityByte = 60;

                //antenna slightly ahead of pivot at minimum distance
                mf.vehicle.antennaPivot = Properties.Vehicle.Default.setVehicle_antennaPivot = 0.1;

                //image to triangle
                mf.isVehicleImage = false;
                Properties.Settings.Default.setDisplay_isVehicleImage = false;

                //front hitch
                mf.tool.isToolRearFixed = Properties.Vehicle.Default.setTool_isToolRearFixed = false;
                mf.tool.isToolTrailing = Properties.Vehicle.Default.setTool_isToolTrailing = false;
                mf.tool.isToolTBT = Properties.Vehicle.Default.setTool_isToolTBT = false;
                mf.tool.isToolFrontFixed = Properties.Vehicle.Default.setTool_isToolFront = true;

                //hitch slightly forward to min length
                Properties.Vehicle.Default.setVehicle_hitchLength = mf.tool.hitchLength = 0.1;
            }
        }

        private void tabASteer_Leave(object sender, EventArgs e)
        {
            mf.pn.isGPSTool = cboxGPSToolOnOff.Checked;
            Properties.Settings.Default.setGPS_isGPSTool = mf.pn.isGPSTool;
            mf.pn.isGPSToolOnly = cboxGPSToolOnlyOnOff.Checked;
            Properties.Settings.Default.setGPS_isGPSToolOnly = mf.pn.isGPSToolOnly;
            
            Properties.Settings.Default.Save();
            Properties.Vehicle.Default.Save();
        }

        #endregion
        private void Enable_AlertM_Click(object sender, EventArgs e)
        {
            pboxSendMachine.Visible = true;
        }

        #region Module Machine

        private void tabAMachine_Enter(object sender, EventArgs e)
        {

            int sett = Properties.Vehicle.Default.setArdMac_setting0;

            if ((sett & 1) == 0) cboxMachInvertRelays.Checked = false;
            else cboxMachInvertRelays.Checked = true;

            if ((sett & 2) == 0) cboxIsHydOn.Checked = false;
            else cboxIsHydOn.Checked = true;

            if (cboxIsHydOn.Checked)
            {
                cboxIsHydOn.Image = Properties.Resources.SwitchOn;
                nudHydLiftLookAhead.Enabled = true;
                nudLowerTime.Enabled = true;
                nudRaiseTime.Enabled = true;
            }
            else
            {
                cboxIsHydOn.Image = Properties.Resources.SwitchOff;
                nudHydLiftLookAhead.Enabled = false;
                nudLowerTime.Enabled = false;
                nudRaiseTime.Enabled = false;
            }

            nudRaiseTime.Value = (decimal)Properties.Vehicle.Default.setArdMac_hydRaiseTime;
            nudLowerTime.Value = (decimal)Properties.Vehicle.Default.setArdMac_hydLowerTime;

            nudUser1.Value = Properties.Vehicle.Default.setArdMac_user1;
            nudUser2.Value = Properties.Vehicle.Default.setArdMac_user2;
            nudUser3.Value = Properties.Vehicle.Default.setArdMac_user3;
            nudUser4.Value = Properties.Vehicle.Default.setArdMac_user4;

            btnSendMachinePGN.Focus();

            nudHydLiftLookAhead.Value = (decimal)Properties.Vehicle.Default.setVehicle_hydraulicLiftLookAhead;
        }
        private void tabAMachine_Leave(object sender, EventArgs e)
        {
            pboxSendMachine.Visible = false;
        }

        private void nudHydLiftSecs_Click(object sender, EventArgs e)
        {
            if (mf.KeypadToNUD((NumericUpDown)sender, this))
            {
                pboxSendMachine.Visible = true;
            }
        }

        private void nudRaiseTime_Click(object sender, EventArgs e)
        {
            if (mf.KeypadToNUD((NumericUpDown)sender, this))
            {
                pboxSendMachine.Visible = true;
            }
        }

        private void nudLowerTime_Click(object sender, EventArgs e)
        {
            if (mf.KeypadToNUD((NumericUpDown)sender, this))
            {
                pboxSendMachine.Visible = true;
            }
        }

        private void nudUser1_Click(object sender, EventArgs e)
        {
            if (mf.KeypadToNUD((NumericUpDown)sender, this))
            {
                pboxSendMachine.Visible = true;
            }
        }

        private void nudUser2_Click(object sender, EventArgs e)
        {
            if (mf.KeypadToNUD((NumericUpDown)sender, this))
            {
                pboxSendMachine.Visible = true;
            }
        }

        private void nudUser3_Click(object sender, EventArgs e)
        {
            if (mf.KeypadToNUD((NumericUpDown)sender, this))
            {
                pboxSendMachine.Visible = true;
            }
        }

        private void nudUser4_Click(object sender, EventArgs e)
        {
            if (mf.KeypadToNUD((NumericUpDown)sender, this))
            {
                pboxSendMachine.Visible = true;
            }
        }
        private void cboxIsHydOn_CheckStateChanged(object sender, EventArgs e)
        {
            if (cboxIsHydOn.Checked)
            {
                cboxIsHydOn.Image = Properties.Resources.SwitchOn;
                nudHydLiftLookAhead.Enabled = true;
                nudLowerTime.Enabled = true;
                nudRaiseTime.Enabled = true;
            }
            else
            {
                cboxIsHydOn.Image = Properties.Resources.SwitchOff;
                nudHydLiftLookAhead.Enabled = false;
                nudLowerTime.Enabled = false;
                nudRaiseTime.Enabled = false;
            }
            //pboxSendMachine.Visible = true;
        }

        private void SaveSettingsMachine()
        {
            int set = 1;
            int reset = 2046;
            int sett = 0;

            if (cboxMachInvertRelays.Checked) sett |= set;
            else sett &= reset;

            set <<= 1;
            reset <<= 1;
            reset += 1;
            if (cboxIsHydOn.Checked) sett |= set;
            else sett &= reset;

            Properties.Vehicle.Default.setArdMac_setting0 = (byte)sett;
            Properties.Vehicle.Default.setArdMac_hydRaiseTime = (byte)nudRaiseTime.Value;
            Properties.Vehicle.Default.setArdMac_hydLowerTime = (byte)nudLowerTime.Value;

            Properties.Vehicle.Default.setArdMac_user1 = (byte)nudUser1.Value;
            Properties.Vehicle.Default.setArdMac_user2 = (byte)nudUser2.Value;
            Properties.Vehicle.Default.setArdMac_user3 = (byte)nudUser3.Value;
            Properties.Vehicle.Default.setArdMac_user4 = (byte)nudUser4.Value;

            Properties.Vehicle.Default.setVehicle_hydraulicLiftLookAhead = (double)nudHydLiftLookAhead.Value;
            mf.vehicle.hydLiftLookAheadTime = Properties.Vehicle.Default.setVehicle_hydraulicLiftLookAhead;

            mf.p_238.pgn[mf.p_238.set0] = (byte)sett;
            mf.p_238.pgn[mf.p_238.raiseTime] = (byte)nudRaiseTime.Value;
            mf.p_238.pgn[mf.p_238.lowerTime] = (byte)nudLowerTime.Value;
            
            mf.p_238.pgn[mf.p_238.user1] = (byte)nudUser1.Value;
            mf.p_238.pgn[mf.p_238.user2] = (byte)nudUser2.Value;
            mf.p_238.pgn[mf.p_238.user3] = (byte)nudUser3.Value;
            mf.p_238.pgn[mf.p_238.user4] = (byte)nudUser4.Value;

            mf.SendPgnToLoop(mf.p_238.pgn);
            pboxSendMachine.Visible = false;
        }

        private void btnSendMachinePGN_Click(object sender, EventArgs e)
        {
            SaveSettingsMachine();

            Properties.Vehicle.Default.Save();

            mf.TimedMessageBox(1000, gStr.gsMachinePort, gStr.gsSentToMachineModule);

            pboxSendMachine.Visible = false;
        }

        #endregion

        #region Relay Config

        private string[] words;

        private void tabRelay_Enter(object sender, EventArgs e)
        {
            pboxSendRelay.Visible = false;

            string[] wordsList = { "-","Section 1","Section 2","Section 3","Section 4","Section 5","Section 6","Section 7",
                    "Section 8","Section 9","Section 10","Section 11","Section 12","Section 13","Section 14","Section 15",
                    "Section 16","Hyd Up","Hyd Down","Tram Right","Tram Left", "Geo Stop"};

            //19 tram right and 20 tram left

            cboxPin0.Items.Clear(); cboxPin0.Items.AddRange(wordsList);
            cboxPin1.Items.Clear(); cboxPin1.Items.AddRange(wordsList);
            cboxPin2.Items.Clear(); cboxPin2.Items.AddRange(wordsList);
            cboxPin3.Items.Clear(); cboxPin3.Items.AddRange(wordsList);
            cboxPin4.Items.Clear(); cboxPin4.Items.AddRange(wordsList);
            cboxPin5.Items.Clear(); cboxPin5.Items.AddRange(wordsList);
            cboxPin6.Items.Clear(); cboxPin6.Items.AddRange(wordsList);
            cboxPin7.Items.Clear(); cboxPin7.Items.AddRange(wordsList);
            cboxPin8.Items.Clear(); cboxPin8.Items.AddRange(wordsList);
            cboxPin9.Items.Clear(); cboxPin9.Items.AddRange(wordsList);
            
            cboxPin10.Items.Clear(); cboxPin10.Items.AddRange(wordsList);
            cboxPin11.Items.Clear(); cboxPin11.Items.AddRange(wordsList);
            cboxPin12.Items.Clear(); cboxPin12.Items.AddRange(wordsList);
            cboxPin13.Items.Clear(); cboxPin13.Items.AddRange(wordsList);
            cboxPin14.Items.Clear(); cboxPin14.Items.AddRange(wordsList);
            cboxPin15.Items.Clear(); cboxPin15.Items.AddRange(wordsList);
            cboxPin16.Items.Clear(); cboxPin16.Items.AddRange(wordsList);
            cboxPin17.Items.Clear(); cboxPin17.Items.AddRange(wordsList);
            cboxPin18.Items.Clear(); cboxPin18.Items.AddRange(wordsList);
            cboxPin19.Items.Clear(); cboxPin19.Items.AddRange(wordsList);

            cboxPin20.Items.Clear(); cboxPin20.Items.AddRange(wordsList);
            cboxPin21.Items.Clear(); cboxPin21.Items.AddRange(wordsList);
            cboxPin22.Items.Clear(); cboxPin22.Items.AddRange(wordsList);
            cboxPin23.Items.Clear(); cboxPin23.Items.AddRange(wordsList);

            words = Properties.Settings.Default.setRelay_pinConfig.Split(',');

            cboxPin0.SelectedIndex = int.Parse(words[0]);
            cboxPin1.SelectedIndex = int.Parse(words[1]);
            cboxPin2.SelectedIndex = int.Parse(words[2]);
            cboxPin3.SelectedIndex = int.Parse(words[3]);
            cboxPin4.SelectedIndex = int.Parse(words[4]);
            cboxPin5.SelectedIndex = int.Parse(words[5]);
            cboxPin6.SelectedIndex = int.Parse(words[6]);
            cboxPin7.SelectedIndex = int.Parse(words[7]);
            cboxPin8.SelectedIndex = int.Parse(words[8]);
            cboxPin9.SelectedIndex = int.Parse(words[9]);
            cboxPin10.SelectedIndex = int.Parse(words[10]);
            cboxPin11.SelectedIndex = int.Parse(words[11]);
            cboxPin12.SelectedIndex = int.Parse(words[12]);
            cboxPin13.SelectedIndex = int.Parse(words[13]);
            cboxPin14.SelectedIndex = int.Parse(words[14]);
            cboxPin15.SelectedIndex = int.Parse(words[15]);
            cboxPin16.SelectedIndex = int.Parse(words[16]);
            cboxPin17.SelectedIndex = int.Parse(words[17]);
            cboxPin18.SelectedIndex = int.Parse(words[18]);
            cboxPin19.SelectedIndex = int.Parse(words[19]);
            cboxPin20.SelectedIndex = int.Parse(words[20]);
            cboxPin21.SelectedIndex = int.Parse(words[21]);
            cboxPin22.SelectedIndex = int.Parse(words[22]);
            cboxPin23.SelectedIndex = int.Parse(words[23]);
        }

        private void tabRelay_Leave(object sender, EventArgs e)
        {
            pboxSendRelay.Visible = false;
        }

        private void btnSendRelayConfigPGN_Click(object sender, EventArgs e)
        {
            SaveSettingsRelay();
            SendRelaySettingsToMachineModule();

            mf.TimedMessageBox(1000, gStr.gsMachinePort, gStr.gsSentToMachineModule);

            pboxSendRelay.Visible = false;
        }

        private void SaveSettingsRelay()
        {
            StringBuilder bob = new StringBuilder();

            bob.Append(cboxPin0.SelectedIndex.ToString() + ",")
               .Append(cboxPin1.SelectedIndex.ToString() + ",")
               .Append(cboxPin2.SelectedIndex.ToString() + ",")
               .Append(cboxPin3.SelectedIndex.ToString() + ",")
               .Append(cboxPin4.SelectedIndex.ToString() + ",")
               .Append(cboxPin5.SelectedIndex.ToString() + ",")
               .Append(cboxPin6.SelectedIndex.ToString() + ",")
               .Append(cboxPin7.SelectedIndex.ToString() + ",")
               .Append(cboxPin8.SelectedIndex.ToString() + ",")
               .Append(cboxPin9.SelectedIndex.ToString() + ",")
               .Append(cboxPin10.SelectedIndex.ToString() + ",")
               .Append(cboxPin11.SelectedIndex.ToString() + ",")
               .Append(cboxPin12.SelectedIndex.ToString() + ",")
               .Append(cboxPin13.SelectedIndex.ToString() + ",")
               .Append(cboxPin14.SelectedIndex.ToString() + ",")
               .Append(cboxPin15.SelectedIndex.ToString() + ",")
               .Append(cboxPin16.SelectedIndex.ToString() + ",")
               .Append(cboxPin17.SelectedIndex.ToString() + ",")
               .Append(cboxPin18.SelectedIndex.ToString() + ",")
               .Append(cboxPin19.SelectedIndex.ToString() + ",")
               .Append(cboxPin20.SelectedIndex.ToString() + ",")
               .Append(cboxPin21.SelectedIndex.ToString() + ",")
               .Append(cboxPin22.SelectedIndex.ToString() + ",")
               .Append(cboxPin23.SelectedIndex.ToString());

            Properties.Settings.Default.setRelay_pinConfig = bob.ToString();

            //save settings
            Properties.Settings.Default.Save();
            pboxSendRelay.Visible = false;

        }

        private void SendRelaySettingsToMachineModule()
        {
            words = Properties.Settings.Default.setRelay_pinConfig.Split(',');

            //load the pgn
            mf.p_236.pgn[mf.p_236.pin0] = (byte)int.Parse(words[0]);
            mf.p_236.pgn[mf.p_236.pin1] = (byte)int.Parse(words[1]);
            mf.p_236.pgn[mf.p_236.pin2] = (byte)int.Parse(words[2]);
            mf.p_236.pgn[mf.p_236.pin3] = (byte)int.Parse(words[3]);
            mf.p_236.pgn[mf.p_236.pin4] = (byte)int.Parse(words[4]);
            mf.p_236.pgn[mf.p_236.pin5] = (byte)int.Parse(words[5]);
            mf.p_236.pgn[mf.p_236.pin6] = (byte)int.Parse(words[6]);
            mf.p_236.pgn[mf.p_236.pin7] = (byte)int.Parse(words[7]);
            mf.p_236.pgn[mf.p_236.pin8] = (byte)int.Parse(words[8]);
            mf.p_236.pgn[mf.p_236.pin9] = (byte)int.Parse(words[9]);
                               
            mf.p_236.pgn[mf.p_236.pin10] = (byte)int.Parse(words[10]);
            mf.p_236.pgn[mf.p_236.pin11] = (byte)int.Parse(words[11]);
            mf.p_236.pgn[mf.p_236.pin12] = (byte)int.Parse(words[12]);
            mf.p_236.pgn[mf.p_236.pin13] = (byte)int.Parse(words[13]);
            mf.p_236.pgn[mf.p_236.pin14] = (byte)int.Parse(words[14]);
            mf.p_236.pgn[mf.p_236.pin15] = (byte)int.Parse(words[15]);
            mf.p_236.pgn[mf.p_236.pin16] = (byte)int.Parse(words[16]);
            mf.p_236.pgn[mf.p_236.pin17] = (byte)int.Parse(words[17]);
            mf.p_236.pgn[mf.p_236.pin18] = (byte)int.Parse(words[18]);
            mf.p_236.pgn[mf.p_236.pin19] = (byte)int.Parse(words[19]);

            mf.p_236.pgn[mf.p_236.pin20] = (byte)int.Parse(words[20]);
            mf.p_236.pgn[mf.p_236.pin21] = (byte)int.Parse(words[21]);
            mf.p_236.pgn[mf.p_236.pin22] = (byte)int.Parse(words[22]);
            mf.p_236.pgn[mf.p_236.pin23] = (byte)int.Parse(words[23]);
            mf.SendPgnToLoop(mf.p_236.pgn);

        }

        private void btnRelaySetDefaultConfig_Click(object sender, EventArgs e)
        {
            pboxSendRelay.Visible = true;

            cboxPin0.SelectedIndex = 1;
            cboxPin1.SelectedIndex = 2;
            cboxPin2.SelectedIndex = 3;
            cboxPin3.SelectedIndex = 0;
            cboxPin4.SelectedIndex = 0;
            cboxPin5.SelectedIndex = 0;
            cboxPin6.SelectedIndex = 0;
            cboxPin7.SelectedIndex = 0;
            cboxPin8.SelectedIndex = 0;
            cboxPin9.SelectedIndex = 0;
            cboxPin10.SelectedIndex = 0;
            cboxPin11.SelectedIndex = 0;
            cboxPin12.SelectedIndex = 0;
            cboxPin13.SelectedIndex = 0;
            cboxPin14.SelectedIndex = 0;
            cboxPin15.SelectedIndex = 0;
            cboxPin16.SelectedIndex = 0;
            cboxPin17.SelectedIndex = 0;
            cboxPin18.SelectedIndex = 0;
            cboxPin19.SelectedIndex = 0;
            cboxPin20.SelectedIndex = 0;
            cboxPin21.SelectedIndex = 0;
            cboxPin22.SelectedIndex = 0;
            cboxPin23.SelectedIndex = 0;
        }

        private void btnRelayResetConfigToNone_Click(object sender, EventArgs e)
        {
            pboxSendRelay.Visible = true;

            cboxPin0.SelectedIndex = 0;
            cboxPin1.SelectedIndex = 0;
            cboxPin2.SelectedIndex = 0;
            cboxPin3.SelectedIndex = 0;
            cboxPin4.SelectedIndex = 0;
            cboxPin5.SelectedIndex = 0;
            cboxPin6.SelectedIndex = 0;
            cboxPin7.SelectedIndex = 0;
            cboxPin8.SelectedIndex = 0;
            cboxPin9.SelectedIndex = 0;
            cboxPin10.SelectedIndex = 0;
            cboxPin11.SelectedIndex = 0;
            cboxPin12.SelectedIndex = 0;
            cboxPin13.SelectedIndex = 0;
            cboxPin14.SelectedIndex = 0;
            cboxPin15.SelectedIndex = 0;
            cboxPin16.SelectedIndex = 0;
            cboxPin17.SelectedIndex = 0;
            cboxPin18.SelectedIndex = 0;
            cboxPin19.SelectedIndex = 0;
            cboxPin20.SelectedIndex = 0;
            cboxPin21.SelectedIndex = 0;
            cboxPin22.SelectedIndex = 0;
            cboxPin23.SelectedIndex = 0;
        }

        private void cboxPin0_Click(object sender, EventArgs e)
        {
            pboxSendRelay.Visible = true;
        }

        #endregion


        #region Uturn Enter-Leave

        private void tabUTurn_Enter(object sender, EventArgs e)
        {
            UpdateUturnText();

            lblSmoothing.Text = mf.yt.uTurnSmoothing.ToString();

            double bob = Properties.Vehicle.Default.set_youTurnDistanceFromBoundary * mf.m2FtOrM;
            if (bob < 0.2) bob = 0.2;
            nudTurnDistanceFromBoundary.Value = (decimal)(Math.Round(bob, 2));

            lblFtMUTurn.Text = mf.unitsFtM;
        }

        private void tabUTurn_Leave(object sender, EventArgs e)
        {
            Properties.Settings.Default.setAS_uTurnSmoothing = mf.yt.uTurnSmoothing;
            Properties.Vehicle.Default.set_youTurnExtensionLength = mf.yt.youTurnStartOffset;

            Properties.Settings.Default.Save();
            Properties.Vehicle.Default.Save();

            mf.bnd.BuildTurnLines();
            mf.yt.ResetCreatedYouTurn();
        }

        #endregion

        #region Uturn controls
        private void UpdateUturnText()
        {
            if (mf.isMetric)
            {
                lblDistance.Text = Math.Abs(mf.yt.youTurnStartOffset).ToString() + " m";
            }
            else
            {
                lblDistance.Text = Math.Abs((int)(mf.yt.youTurnStartOffset * glm.m2ft)).ToString() + " ft";
            }
        }

        private void nudTurnDistanceFromBoundary_Click(object sender, EventArgs e)
        {
            if (mf.KeypadToNUD((NumericUpDown)sender, this))
            {
                mf.yt.uturnDistanceFromBoundary = (double)nudTurnDistanceFromBoundary.Value * mf.ftOrMtoM;
                Properties.Vehicle.Default.set_youTurnDistanceFromBoundary = mf.yt.uturnDistanceFromBoundary;
            }
        }

        private void btnTriggerDistanceDn_Click(object sender, EventArgs e)
        {
            mf.yt.uturnDistanceFromBoundary--;
            if (mf.yt.uturnDistanceFromBoundary < 0.1) mf.yt.uturnDistanceFromBoundary = 0.1;
            UpdateUturnText();
        }

        private void btnTriggerDistanceUp_Click(object sender, EventArgs e)
        {
            if (mf.yt.uturnDistanceFromBoundary++ > 50) mf.yt.uturnDistanceFromBoundary = 50;
            UpdateUturnText();
        }

        private void btnDistanceDn_Click(object sender, EventArgs e)
        {
            if (mf.yt.youTurnStartOffset-- < 4) mf.yt.youTurnStartOffset = 3;
            UpdateUturnText();
        }

        private void btnDistanceUp_Click(object sender, EventArgs e)
        {
            if (mf.yt.youTurnStartOffset++ > 49) mf.yt.youTurnStartOffset = 50;
            UpdateUturnText();
        }
        private void btnTurnSmoothingDown_Click(object sender, EventArgs e)
        {
            mf.yt.uTurnSmoothing -= 2;
            if (mf.yt.uTurnSmoothing < 4) mf.yt.uTurnSmoothing = 4;
            lblSmoothing.Text = mf.yt.uTurnSmoothing.ToString();
        }

        private void btnTurnSmoothingUp_Click(object sender, EventArgs e)
        {
            mf.yt.uTurnSmoothing += 2;
            if (mf.yt.uTurnSmoothing > 18) mf.yt.uTurnSmoothing = 18;
            lblSmoothing.Text = mf.yt.uTurnSmoothing.ToString();
        }

        #endregion

        #region Tram
        private void tabTram_Enter(object sender, EventArgs e)
        {
            lblTramWidthUnits.Text = mf.unitsInCm;

            nudTramWidth.Value = (int)(Math.Abs(Properties.Settings.Default.setTram_tramWidth) * mf.m2InchOrCm);

            cboxTramOnBackBuffer.Checked = Properties.Settings.Default.setTram_isTramOnBackBuffer;
        }

        private void tabTram_Leave(object sender, EventArgs e)
        {

            if (cboxTramOnBackBuffer.Checked) Properties.Settings.Default.setTram_isTramOnBackBuffer = true;
            else Properties.Settings.Default.setTram_isTramOnBackBuffer = false;
            mf.isTramOnBackBuffer = Properties.Settings.Default.setTram_isTramOnBackBuffer;

            mf.tram.isOuter = ((int)(mf.tram.tramWidth / mf.tool.toolWidth + 0.5)) % 2 == 0 ? true : false;

            Properties.Settings.Default.Save();
            
        }
        private void nudTramWidth_Click(object sender, EventArgs e)
        {
            if (mf.KeypadToNUD((NumericUpDown)sender, this))
            {
                mf.tram.tramWidth = (double)nudTramWidth.Value * mf.inchOrCm2m;
                Properties.Settings.Default.setTram_tramWidth = mf.tram.tramWidth;
            }
        }

        #endregion

    }
}

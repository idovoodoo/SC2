/*
Copyright (c) 2008 - 2009, De Montfort University
All rights reserved.

Redistribution and use in source and binary forms, with or without modification,
is not permitted.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR
ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
(INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON
ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

*/
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace SnapClutch.Modes
{
    public class ModeSet
    {
        private ModeCollection profileA_Windows;
        private ModeCollection profileB_MMORPGa;
        private ModeCollection profileC_MMORPGb;
        private ModeCollection profileD_switch;
        private ModeCollection profileE_GestureExperiment;
        private ModeCollection profileF_FPSa;
        private ModeCollection profileG_LocoExperiment;

        private ModeCollection activeModeCollection;

        private XmlNodeList xmlGameData;
        private List<CompatibleGame> listOfGames;
        private int numberOfGames;

        public ModeSet()
        {
            InitialiseMyComponents();            
        }

        private void InitialiseMyComponents()
        {
            profileA_Windows = ProfileCollections.GetProfileA_Windows();
            profileB_MMORPGa = ProfileCollections.GetProfileB_MMORPGa();
            profileC_MMORPGb = ProfileCollections.GetProfileC_MMORPGb();
            profileD_switch = ProfileCollections.GetProfileD_Switch();
            profileE_GestureExperiment = ProfileCollections.GetProfileE_GestureExperiment();
            profileF_FPSa = ProfileCollections.GetProfileF_BlinkAttack();
            profileG_LocoExperiment = ProfileCollections.GetProfileG_LocoExperiment();
        }


        // populate compatible games from xml file
        public void PopulateFromXml()
        {
            listOfGames = new List<CompatibleGame>();

            XmlDocument xmlDocument = new XmlDocument();
            //Console.WriteLine("*** Populating from XML file ***");
            xmlDocument.Load("games.xml");

            // parse game data
            //Console.WriteLine("*** Parsing game data ***");
            xmlGameData = xmlDocument.GetElementsByTagName("game");

            foreach (XmlNode node in xmlGameData)
            {
                XmlElement gameElement = (XmlElement)node;
                CompatibleGame game = new CompatibleGame(gameElement.GetAttribute("name"), gameElement.GetAttribute("handle"),
                    gameElement.GetAttribute("profile_a"), gameElement.GetAttribute("profile_b"));
                numberOfGames++;

                listOfGames.Add(game);
            }

            //foreach (CompatibleGame game in listOfGames)
            //{
            //    Console.WriteLine(game.GetName());
            //}

            //Console.WriteLine("*** Import finished ***");
            //Console.WriteLine("*** Number of games = {0}", numberOfGames);
        }

        public void ActivateGameProfile(int gameNumber, int profileNumber)
        {
            if (profileNumber == 1)
                activeModeCollection = listOfGames[gameNumber].GetProfile1();
            else if (profileNumber == 2)
                activeModeCollection = listOfGames[gameNumber].GetProfile2();
        }

        public List<CompatibleGame> GetGameList()
        {
            return listOfGames;
        }

        public void ActivateProfileA_Windows()
        {

            activeModeCollection = profileA_Windows;
        }

        public void ActivateProfileB_MMORPGa()
        {
            activeModeCollection = profileB_MMORPGa;
        }

        public void ActivateProfileC_MMORPGb()
        {
            activeModeCollection = profileC_MMORPGb;
        }

        public void ActivateProfileD_Switch()
        {
            activeModeCollection = profileD_switch;
        }

        public void ActivateProfileE_GestureExperiment()
        {
            activeModeCollection = profileE_GestureExperiment;
        }

        public void ActivateProfileF_FPSa()
        {
            activeModeCollection = profileF_FPSa;
        }

        public void ActivateProfileG_LocoExperiment()
        {
            activeModeCollection = profileG_LocoExperiment;
        }

        public ModeCollection GetActiveModeCollection()
        {
            return activeModeCollection;
        }

        public bool IsProfileAActive_Windows()
        {
            if (activeModeCollection == profileA_Windows)
                return true;
            else
                return false;
        }

        public bool IsProfileBActive_MMORPGa()
        {
            if (activeModeCollection == profileB_MMORPGa)
                return true;
            else
                return false;
        }

        public bool IsProfileCActive_MMORPGb()
        {
            if (activeModeCollection == profileC_MMORPGb)
                return true;
            else
                return false;
        }

        public bool IsProfileDActive_Switch()
        {
            if (activeModeCollection == profileD_switch)
                return true;
            else
                return false;
        }

        public bool IsProfileEActive_GestureExperiment()
        {
            if (activeModeCollection == profileE_GestureExperiment)
                return true;
            else
                return false;
        }

        public bool IsProfileFActive_FPSa()
        {
            if (activeModeCollection == profileF_FPSa)
                return true;
            else
                return false;
        }

        public bool IsProfileGActive_LocoExperiment()
        {
            if (activeModeCollection == profileG_LocoExperiment)
                return true;
            else
                return false;
        }
    }
}

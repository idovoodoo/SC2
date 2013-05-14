using System;
using System.Collections.Generic;
using System.Text;

namespace SnapClutch.Modes
{
    public class CompatibleGame
    {
        string name;
        string handle;
        ModeSet myModeSet = new ModeSet();

        ModeCollection modeCollectionProfile1, modeCollectionProfile2;
        

        public CompatibleGame(string argName, string argHandle, string argProA, string argProB)
        {
            name = argName;
            handle = argHandle;

            switch (argProA)
            {
                case "pro1":
                    modeCollectionProfile1 = ProfileCollections.GetProfileA_Windows();
                    break;
                case "pro2":
                    modeCollectionProfile1 = ProfileCollections.GetProfileB_MMORPGa();
                    break;
                case "pro3":
                    modeCollectionProfile1 = ProfileCollections.GetProfileC_MMORPGb();
                    break;
                case "pro4":
                    modeCollectionProfile1 = ProfileCollections.GetProfileD_Switch();
                    break;
                case "pro5":
                    modeCollectionProfile1 = ProfileCollections.GetProfileE_GestureExperiment();
                    break;
                case "pro6":
                    modeCollectionProfile1 = ProfileCollections.GetProfileF_BlinkAttack();
                    break;
                case "pro7":
                    modeCollectionProfile1 = ProfileCollections.GetProfileG_LocoExperiment();
                    break;
                case "none":
                    modeCollectionProfile1 = ProfileCollections.GetProfileA_Windows();
                    break;                    
            }

            switch (argProB)
            {
                case "pro1":
                    modeCollectionProfile2 = ProfileCollections.GetProfileA_Windows();
                    break;
                case "pro2":
                    modeCollectionProfile2 = ProfileCollections.GetProfileB_MMORPGa();
                    break;
                case "pro3":
                    modeCollectionProfile2 = ProfileCollections.GetProfileC_MMORPGb();
                    break;
                case "pro4":
                    modeCollectionProfile2 = ProfileCollections.GetProfileD_Switch();
                    break;
                case "pro5":
                    modeCollectionProfile2 = ProfileCollections.GetProfileE_GestureExperiment();
                    break;
                case "pro6":
                    modeCollectionProfile2 = ProfileCollections.GetProfileF_BlinkAttack();
                    break;
                case "pro7":
                    modeCollectionProfile2 = ProfileCollections.GetProfileG_LocoExperiment();
                    break;
                case "none":
                    modeCollectionProfile2 = modeCollectionProfile1;
                    break;
            }
        }

        public string GetName()
        {
            return name;
        }

        public string GetHandle()
        {
            return handle;
        }

        public ModeCollection GetProfile1()
        {
            return modeCollectionProfile1; ;
        }

        public ModeCollection GetProfile2()
        {
            return modeCollectionProfile2;
        }
    }
}

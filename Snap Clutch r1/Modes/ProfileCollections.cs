using System;
using System.Collections.Generic;
using System.Text;

namespace SnapClutch.Modes
{
    public class ProfileCollections
    {
        private static ModeCollection profileA_Windows;// windowsModeCollection;
        private static ModeCollection profileB_MMORPGa;//mmorpgModeCollection;
        private static ModeCollection profileC_MMORPGb;//MMORPGModeCollectionVariant2;
        private static ModeCollection profileD_switch;//switchModeCollection;
        private static ModeCollection profileE_GestureExperiment;//gestureExperimentCollection;
        private static ModeCollection profileF_FPSa;//FPSExperimentCollection;
        private static ModeCollection profileG_LocoExperiment;

        private static ModeDwellClickLeft myModeDwellClickLeft;
        private static ModeDwellClickRight myModeDwellClickRight;
        private static ModeOffSmallCursor myModeOffSmallCursor;
        private static ModeLeftDrag myModeLeftDrag;
        private static ModeKeyLookAroundCatA myModeOffLookAround;
        private static ModeKeyLocomotionCatA myModeLocomotion;
        private static ModeMouseLookAroundCatA myModeMouseLookAround;
        private static ModeSnapClutch myModeSnapClutch;
        private static ModeMouseLocomotionCatA myModeMouseLocomotion;
        private static ModeSwitchLeftClick myModeSwitchLeftClick;
        private static ModeCameraControl myModeCameraControl;
        private static ModeMouseCameraControl myModeMouseCameraControl;
        private static ModeLocomotionBlinkAttack myModeLocomotionBlinkAttack;
        private static ModeJoystickLocomotion myModeJoystickLocomotion;
        private static ModeMouseLocomotionFPS myModeMouseLocomotionFPS;
        private static ModeKeyLocomotionCatB myModeLocomotionCat2;
        private static ModeKeyLookAroundCatB myModeOffLookAroundCat2;
        private static ModeMouseLocomotionCatB myModeMouseLocomotionCat2;
        private static ModeMouseLookAroundCatB myModeMouseLookAroundCat2;
        private static ModeLocomotionCatC myModeLocomotionCat3;
        private static ModeOffLookAroundCatC myModeOffLookAroundCat3;
        
        // mouse cat a
        public static ModeCollection GetProfileA_Windows()
        {
            InitialiseCursors();

            profileA_Windows = new ModeCollection();

            //profileA_Windows.SetModeBottom(myModeOffSmallCursor);
            //profileA_Windows.SetModeLeft(myModeDwellClickLeft);
            //profileA_Windows.SetModeRight(myModeDwellClickRight);
            //profileA_Windows.SetModeTop(myModeLeftDrag);

            //profileA_Windows.SetModeBottom2(myModeOffSmallCursor);
            //profileA_Windows.SetModeLeft2(myModeDwellClickLeft);
            //profileA_Windows.SetModeRight2(myModeDwellClickRight);
            //profileA_Windows.SetModeTop2(myModeLeftDrag);

            profileA_Windows.SetModeBottom(myModeMouseLookAround);
            profileA_Windows.SetModeLeft(myModeMouseLocomotion);
            profileA_Windows.SetModeRight(myModeMouseLocomotion);
            profileA_Windows.SetModeTop(myModeMouseLocomotion);

            profileA_Windows.SetModeBottom2(myModeMouseLookAround);
            profileA_Windows.SetModeLeft2(myModeMouseLocomotion);
            profileA_Windows.SetModeRight2(myModeMouseLocomotion);
            profileA_Windows.SetModeTop2(myModeMouseLocomotion);

            return profileA_Windows;
        }

        // keyboard cat a
        public static ModeCollection GetProfileB_MMORPGa()
        {
            InitialiseCursors();

            profileB_MMORPGa = new ModeCollection();

            profileB_MMORPGa.SetModeBottom(myModeOffLookAround);
            profileB_MMORPGa.SetModeLeft(myModeLocomotion);
            profileB_MMORPGa.SetModeRight(myModeLocomotion);
            profileB_MMORPGa.SetModeTop(myModeLocomotion);

            profileB_MMORPGa.SetModeBottom2(myModeOffLookAround);
            profileB_MMORPGa.SetModeLeft2(myModeLocomotion);
            profileB_MMORPGa.SetModeRight2(myModeLocomotion);
            profileB_MMORPGa.SetModeTop2(myModeLocomotion);

            //profileB_MMORPGa.SetModeBottom(myModeOffLookAroundCat2);
            //profileB_MMORPGa.SetModeLeft(myModeLocomotionCat2);
            //profileB_MMORPGa.SetModeRight(myModeLocomotionCat2);
            //profileB_MMORPGa.SetModeTop(myModeLocomotionCat2);

            //profileB_MMORPGa.SetModeBottom2(myModeOffLookAroundCat2);
            //profileB_MMORPGa.SetModeLeft2(myModeLocomotionCat2);
            //profileB_MMORPGa.SetModeRight2(myModeLocomotionCat2);
            //profileB_MMORPGa.SetModeTop2(myModeLocomotionCat2);

            return profileB_MMORPGa;
        }

        // keyboard cat b
        public static ModeCollection GetProfileC_MMORPGb()
        {
            InitialiseCursors();

            profileC_MMORPGb = new ModeCollection();

            profileC_MMORPGb.SetModeBottom(myModeOffLookAroundCat3);
            profileC_MMORPGb.SetModeLeft(myModeLocomotionCat3);
            profileC_MMORPGb.SetModeRight(myModeLocomotionCat3);
            profileC_MMORPGb.SetModeTop(myModeLocomotionCat3);

            profileC_MMORPGb.SetModeBottom2(myModeOffLookAroundCat3);
            profileC_MMORPGb.SetModeLeft2(myModeLocomotionCat3);
            profileC_MMORPGb.SetModeRight2(myModeLocomotionCat3);
            profileC_MMORPGb.SetModeTop2(myModeLocomotionCat3);

            return profileC_MMORPGb;
        }

        // keyboard cat c
        public static ModeCollection GetProfileD_Switch()
        {
            InitialiseCursors();

            profileD_switch = new ModeCollection();

            profileD_switch.SetModeBottom(myModeMouseLookAroundCat2);
            profileD_switch.SetModeLeft(myModeMouseLocomotionCat2);
            profileD_switch.SetModeRight(myModeMouseLocomotionCat2);
            profileD_switch.SetModeTop(myModeMouseLocomotionCat2);

            profileD_switch.SetModeBottom2(myModeMouseLookAroundCat2);
            profileD_switch.SetModeLeft2(myModeMouseLocomotionCat2);
            profileD_switch.SetModeRight2(myModeMouseLocomotionCat2);
            profileD_switch.SetModeTop2(myModeMouseLocomotionCat2);

            return profileD_switch;
        }
        
        public static ModeCollection GetProfileE_GestureExperiment()
        {
            InitialiseCursors();

            profileE_GestureExperiment = new ModeCollection();

            profileE_GestureExperiment.SetModeBottom(myModeMouseLookAround);
            profileE_GestureExperiment.SetModeLeft(myModeDwellClickLeft);
            profileE_GestureExperiment.SetModeRight(myModeDwellClickRight);
            profileE_GestureExperiment.SetModeTop(myModeMouseLocomotion);

            profileE_GestureExperiment.SetModeBottom2(myModeMouseCameraControl);
            profileE_GestureExperiment.SetModeLeft2(myModeDwellClickLeft);
            profileE_GestureExperiment.SetModeRight2(myModeDwellClickRight);
            profileE_GestureExperiment.SetModeTop2(myModeMouseLocomotion);

            return profileE_GestureExperiment;
        }

        public static ModeCollection GetProfileF_BlinkAttack()
        {
            InitialiseCursors();

            profileF_FPSa = new ModeCollection();

            profileF_FPSa.SetModeBottom(myModeMouseLookAround);
            profileF_FPSa.SetModeLeft(myModeDwellClickLeft);
            profileF_FPSa.SetModeRight(myModeDwellClickRight);
            profileF_FPSa.SetModeTop(myModeLocomotionBlinkAttack);

            profileF_FPSa.SetModeBottom2(myModeMouseCameraControl);
            profileF_FPSa.SetModeLeft2(myModeDwellClickLeft);
            profileF_FPSa.SetModeRight2(myModeDwellClickRight);
            profileF_FPSa.SetModeTop2(myModeLocomotionBlinkAttack);

            return profileF_FPSa;
        }

        public static ModeCollection GetProfileG_LocoExperiment()
        {
            InitialiseCursors();

            profileG_LocoExperiment = new ModeCollection();

            profileG_LocoExperiment.SetModeBottom(myModeMouseLookAround);
            profileG_LocoExperiment.SetModeLeft(myModeDwellClickLeft);
            profileG_LocoExperiment.SetModeRight(myModeDwellClickRight);
            profileG_LocoExperiment.SetModeTop(myModeJoystickLocomotion);

            profileG_LocoExperiment.SetModeBottom2(myModeMouseCameraControl);
            profileG_LocoExperiment.SetModeLeft2(myModeDwellClickLeft);
            profileG_LocoExperiment.SetModeRight2(myModeDwellClickRight);
            profileG_LocoExperiment.SetModeTop2(myModeJoystickLocomotion);

            return profileG_LocoExperiment;
        }

        private static void InitialiseCursors()
        {
            myModeDwellClickLeft = new ModeDwellClickLeft();
            myModeDwellClickRight = new ModeDwellClickRight();
            myModeOffSmallCursor = new ModeOffSmallCursor();
            myModeLeftDrag = new ModeLeftDrag();
            myModeOffLookAround = new ModeKeyLookAroundCatA();
            myModeLocomotion = new ModeKeyLocomotionCatA();
            myModeMouseLookAround = new ModeMouseLookAroundCatA();
            myModeSnapClutch = new ModeSnapClutch();
            myModeMouseLocomotion = new ModeMouseLocomotionCatA();
            myModeCameraControl = new ModeCameraControl();
            myModeMouseCameraControl = new ModeMouseCameraControl();
            myModeLocomotionBlinkAttack = new ModeLocomotionBlinkAttack();
            myModeJoystickLocomotion = new ModeJoystickLocomotion();
            myModeMouseLocomotionFPS = new ModeMouseLocomotionFPS();
            myModeLocomotionCat2 = new ModeKeyLocomotionCatB();
            myModeOffLookAroundCat2 = new ModeKeyLookAroundCatB();
            myModeMouseLocomotionCat2 = new ModeMouseLocomotionCatB();
            myModeMouseLookAroundCat2 = new ModeMouseLookAroundCatB();
            myModeLocomotionCat3 = new ModeLocomotionCatC();
            myModeOffLookAroundCat3 = new ModeOffLookAroundCatC();
        }
    }
}

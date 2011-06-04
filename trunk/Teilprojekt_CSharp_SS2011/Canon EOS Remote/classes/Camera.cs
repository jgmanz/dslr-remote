﻿using System;
/*
 * Added on 09-05-2011 12:38 to simplify using of the canon sdk.
 * Now, every command , EDSDKLib namespace havent to be written.
 * */
using EDSDKLib;
using System.ComponentModel;
using System.Collections.Generic;
using Canon_EOS_Remote.classes; 

namespace Canon_EOS_Remote
{
    class Camera : INotifyPropertyChanged
    {
        #region Declaration of class members
        /**
         * Added 05-05-2011 11:50
         * basic properties
         * */
        private IntPtr _cameraPtr;
        private string _cameraName;/* The product name of the camera body*/

        public string CameraName
        {
            get { return _cameraName; }
            set { _cameraName = value; }
        }
        private string _cameraOwner; /*The setted name of the camera owner*/
        private string _cameraBodyID;
        private EdsTime _cameraTime;
        private UInt32 _cameraBatteryLevel;
        private UInt32 _cameraAEMode;
        private UInt32 _cameraDriveMode;
        private UInt32 _cameraISOSpeed;
        private UInt32 _cameraMeteringMode;
        private UInt32 _cameraAFMode;
        private UInt32 _cameraAperture;
        private UInt32 _cameraShutterTime;
        private UInt32 _cameraExposureCompensation;
        private UInt32 _cameraAvailableShots;
        private string _currentStorage;
        private UInt32 tmpErrorCodeAfterCommand;
        private string tmpErrorString;
        private EDSDK.EdsTime _cameraFirmware;
        private bool _lensAttached;
        public event PropertyChangedEventHandler PropertyChanged;
        private EDSDK.EdsPropertyEventHandler cameraPropertyEventHandler;
        private DriveModes driveModes;
        private ISOSpeeds isoList;
        private PropertyCodes propertyCodes;
        private EventCodes eventIDs;
        #endregion

        #region Setter and Getter of class member
        public IntPtr CameraPtr
        {
            get { return _cameraPtr; }
            set { _cameraPtr = value;
            update("_cameraPtr");
            }
        }    

        public string CameraOwner
        {
            get { return _cameraOwner; }
            set { _cameraOwner = value;
            update("_cameraOwner");
            }
        }        

        public string CameraBodyID
        {
            get { return _cameraBodyID; }
            set { _cameraBodyID = value;
            update("_cameraBodyID");
            }
        }

        public EdsTime CameraTime
        {
            get { return _cameraTime; }
            set { _cameraTime = value;
            update("_cameraTime");
            }
        }

        public UInt32 CameraBatteryLevel
        {
            get { return _cameraBatteryLevel; }
            set { _cameraBatteryLevel = value;
            update("_cameraBatteryLeveL");
            }
        }

        public UInt32 CameraAEMode
        {
            get { return _cameraAEMode; }
            set { _cameraAEMode = value;
            update("_cameraAEMode");
            }
        }

        public UInt32 CameraDriveMode
        {
            get { return _cameraDriveMode; }
            set { _cameraDriveMode = value;
            update("_cameraDriveMode");
            }
        }

        public UInt32 CameraISOSpeed
        {
            get { return _cameraISOSpeed; }
            set { _cameraISOSpeed = value;
            update("_cameraISOSpeed");
            }
        }

        public UInt32 CameraMeteringMode
        {
            get { return _cameraMeteringMode; }
            set { _cameraMeteringMode = value;
            update("_cameraMeteringMode");
            }
        }

        public UInt32 CameraAFMode
        {
            get { return _cameraAFMode; }
            set { _cameraAFMode = value;
            update("_cameraAFMode");
            }
        }

        public UInt32 CameraAperture
        {
            get { return _cameraAperture; }
            set { _cameraAperture = value;
            update("_cameraAperture");
            }
        }

        public UInt32 CameraShutterTime
        {
            get { return _cameraShutterTime; }
            set { _cameraShutterTime = value;
            update("_cameraShutterTime");
            }
        }

        public UInt32 CameraExposureCompensation
        {
            get { return _cameraExposureCompensation; }
            set { _cameraExposureCompensation = value;
            update("_cameraExposureCompensation");
            }
        }

        public UInt32 CameraAvailableShots
        {
            get { return _cameraAvailableShots; }
            set { _cameraAvailableShots = value;
            update("_cameraAvailableShots");
            }
        }

        public string CurrentStorage
        {
            get { return _currentStorage; }
            set { _currentStorage = value;
            update("_currentStorage");
            }
        }    

        public EDSDK.EdsTime CameraFirmware
        {
            get { return _cameraFirmware; }
            set { _cameraFirmware = value;
            update("_cameraFirmware");
            }
        }
        #endregion

        #region Constructors

        public Camera(IntPtr cameraPtr)
        {
            throw new NotImplementedException();
        }

        public Camera(IntPtr cameraPtr, String cameraName)
        {
            UInt32 tmpProperty = 0;
            UInt32 error = 0;
            this.CameraPtr = cameraPtr;
            this.CameraName = cameraName;
            this.driveModes = new DriveModes();
            this.isoList = new ISOSpeeds();
            this.propertyCodes = new PropertyCodes();
            this.eventIDs = new EventCodes();
            this.cameraPropertyEventHandler = new EDSDK.EdsPropertyEventHandler(onCameraPropertyChanged);
            if (error != 0)
            {
                System.Windows.MessageBox.Show("Cant register property event handler because : " + error);
            }
            error = EDSDK.EdsOpenSession(this.CameraPtr);
            System.Windows.MessageBox.Show("Cant open session with camera because : " + error);
            getCameraBatteryLevelFromBody();
            error=EDSDK.EdsGetPropertyData(this.CameraPtr, EDSDK.PropID_AEMode, 0, out tmpProperty);
            this.CameraAEMode = tmpProperty;
            error=EDSDK.EdsGetPropertyData(this.CameraPtr, EDSDK.PropID_DriveMode, 0, out tmpProperty);
            this.CameraDriveMode = tmpProperty;
            error=EDSDK.EdsGetPropertyData(this.CameraPtr, EDSDK.PropID_AFMode, 0, out tmpProperty);
            this.CameraAFMode = tmpProperty;
            error=EDSDK.EdsGetPropertyData(this.CameraPtr, EDSDK.PropID_MeteringMode, 0, out tmpProperty);
            this.CameraMeteringMode = tmpProperty;
            error=EDSDK.EdsGetPropertyData(this.CameraPtr, EDSDK.PropID_Tv, 0, out tmpProperty);
            this.CameraShutterTime = tmpProperty;
            error = EDSDK.EdsGetPropertyData(this.CameraPtr, EDSDK.PropID_ISOSpeed, 0, out tmpProperty);
            this.CameraISOSpeed = tmpProperty;
            //error = EDSDK.EdsCloseSession(this.CameraPtr);
            System.Windows.MessageBox.Show("Cant close session with camera because : " + error);
            string tmpString = this.driveModes.getDriveModeString(this.CameraDriveMode);
            System.Windows.MessageBox.Show("Got following properties : \n"+
                "Batterielevel : " + this.CameraBatteryLevel + "%" + "\n" +
                "AEMode : " + this.CameraAEMode + "\n" + 
                "DriveMode : " + this.CameraDriveMode + " = " + this.driveModes.getDriveModeString(this.CameraDriveMode) + "\n" +
                "AFMode : " + this.CameraAFMode + "\n" +
                "MeteringMode : " + this.CameraMeteringMode + "\n" +
                "Tv : " + this.CameraShutterTime + "\n" + 
                "ISO : " + this.CameraISOSpeed + " = " + this.isoList.getISOSpeedFromHex(this.CameraISOSpeed)
                );
            //error = EDSDK.EdsSendCommand(this.CameraPtr, EDSDK.CameraState_UILock, 0);

            error = EDSDK.EdsSetPropertyEventHandler(this.CameraPtr, EDSDK.PropertyEvent_All, this.cameraPropertyEventHandler, _cameraPtr);
        }

        #endregion

        /*
         * Added on 09-05-2011 12:40 to generalize the exception handling,
         * e.g. to get all eds error code informations
         * */
        private string getErrorString(uint errorCodeNumber)
        {
            EdsError tmpError = new EdsError(tmpErrorCodeAfterCommand);
            tmpErrorString = "ErrorNumber : " + tmpError.ErrorCodeNumber + "\n CodeString : " +
            tmpError.ErrorCodeString + "\n Description : " + tmpError.ErrorDescription;
            tmpError = null; /* To dispose the garbage collector to delete this allocation */
            return tmpErrorString;
        }

        private void update(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
                System.Windows.MessageBox.Show("Property has changed from : " + this + " : " + property);
            }
        }

        
        #region camera methods

        private void updateCameraProperties()
        {
            getCameraOwnerFromBody();
        }

        private void getCameraNameFromBody()
        {
            string tmpCameraName="";
            tmpErrorCodeAfterCommand = 0;
            tmpErrorCodeAfterCommand = EDSDK.EdsGetPropertyData(this._cameraPtr, EDSDKLib.EDSDK.PropID_ProductName, 0, out tmpCameraName);
            if (tmpErrorCodeAfterCommand != 0)
            {
                throw new Exception("Command execution not succesfull because :" + getErrorString(tmpErrorCodeAfterCommand));
            }
            else
            {
                _cameraName = tmpCameraName;
            }
        }

        private void getCameraOwnerFromBody()
        {
            string tmpCameraOwner = "";
            tmpErrorCodeAfterCommand = 0;
            tmpErrorCodeAfterCommand = EDSDK.EdsGetPropertyData(this._cameraPtr, EDSDKLib.EDSDK.PropID_OwnerName, 0, out tmpCameraOwner);
            if (tmpErrorCodeAfterCommand != 0)
            {
                throw new Exception("Command execution not succesfull because :" + getErrorString(tmpErrorCodeAfterCommand));
            }
            else
            {
                this.CameraOwner = tmpCameraOwner;
            }
        }

        private void getCameraBodyIDFromBody()
        {
            string tmpCameraBodyID="";
            tmpErrorCodeAfterCommand = 0;
            tmpErrorCodeAfterCommand = EDSDK.EdsGetPropertyData(this._cameraPtr, EDSDKLib.EDSDK.PropID_BodyIDEx, 0, out tmpCameraBodyID);
            if (tmpErrorCodeAfterCommand != 0)
            {
                throw new Exception("Command execution not succesfull because :" + getErrorString(tmpErrorCodeAfterCommand));
            }
            else
            {
                this.CameraBodyID = tmpCameraBodyID;
            }
        }

        private void getCameraBatteryLevelFromBody()
        {
            UInt32 tmpCameraBatteryLevel = 0;
            tmpErrorCodeAfterCommand = 0;
            tmpErrorCodeAfterCommand = EDSDK.EdsGetPropertyData(this._cameraPtr, EDSDKLib.EDSDK.PropID_BatteryLevel, 0, out tmpCameraBatteryLevel);
            if (tmpErrorCodeAfterCommand != 0)
            {
                throw new Exception("Command execution not succesfull because :" + getErrorString(tmpErrorCodeAfterCommand));
            }
            else
            {
                this.CameraBatteryLevel = tmpCameraBatteryLevel;
            }
        }

        private void getCameraTime()
        {
            /*
             * @TODO 
             */
        }

        private uint onCameraPropertyChanged(uint inEvent, uint inPropertyID, uint inParameter, IntPtr inContext)
        {
            System.Windows.MessageBox.Show("CameraPropertey changed : " + 
                "EventID : " + this.eventIDs.getEventIDString(inEvent) + "\nPropID : " + this.propertyCodes.getPropertyString(inPropertyID) + "\nParameter : " + inParameter + "\nContext : " + inContext);
            return 0x0;
        }
        #endregion

    }
}
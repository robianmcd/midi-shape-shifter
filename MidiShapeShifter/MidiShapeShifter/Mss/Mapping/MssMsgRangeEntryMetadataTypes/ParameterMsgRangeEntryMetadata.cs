using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;

using MidiShapeShifter.Mss.Generator;
using MidiShapeShifter.Mss.Parameters;

namespace MidiShapeShifter.Mss.Mapping.MssMsgRangeEntryMetadataTypes
{
    //TODO: comment this class
    public class ParameterMsgRangeEntryMetadata : MssMsgRangeEntryMetadata
    {
        public override MssMsgType MsgType
        {
            get { return MssMsgType.Parameter; }
        }

        protected IMssParameterViewer parameterViewer;

        public void Init(IMssParameterViewer parameterViewer)
        {
            this.parameterViewer = parameterViewer;
        }

        public override Control EntryField1
        {
            get
            {
                if (this.ioCatagory == IoType.Input)
                {
                    return this.mappingDlg.inEntryField1Combo;
                }
                else if (this.ioCatagory == IoType.Output)
                {
                    return this.mappingDlg.outEntryField1Combo;
                }
                else
                {
                    //unexpected ioCatagory
                    Debug.Assert(false);
                    return null;
                }
            }
        }

        public override bool SetData1RangeFromField(out string errorMsg)
        {
            int SelectedIndex = ((ComboBox)EntryField1).SelectedIndex;
            if (SelectedIndex < 0)
            {
                //I don't think this should ever occur as the first parameter should be selected
                //by default....but just to be safe.
                errorMsg = "You must select a parameter.";
                return false;
            }
            else
            {
                MssParameterID paramId = MssParameters.ALL_PARAMS_ID_LIST[SelectedIndex];
                this.msgRange.Data1RangeBottom = (int)paramId;
                this.msgRange.Data1RangeTop = (int)paramId;

                errorMsg = "";
                return true;
            }
        }

        protected override void SetEntryField1FromRange(IMssMsgRange msgRange)
        {
            int paramIndex = MssParameters.ALL_PARAMS_ID_LIST.FindIndex(
                paramId => paramId == (MssParameterID)msgRange.Data1RangeBottom);

            ((ComboBox)EntryField1).SelectedIndex = paramIndex;
        }

        public override Control EntryField2
        {
            get
            {
                return null;
            }
        }

        protected override void InitSameAsInputCompatibleTypes()
        {
            
        }

        protected override void SetMappingDlgEntryFieldCustomProperties()
        {
            this.EntryField1Lbl.Visible = true;
            this.EntryField1Lbl.Text = "Parameter Name:";
            this.EntryField1.Visible = true;

            for (int i = 0; i < MssParameters.ALL_PARAMS_ID_LIST.Count; i++)
            {
                MssParameterID curId = MssParameters.ALL_PARAMS_ID_LIST[i];
                MssParamInfo curParamInfo = this.parameterViewer.GetParameterInfoCopy(curId);
                ((ComboBox)EntryField1).Items.Add(curParamInfo.Name);
            }

            ((ComboBox)EntryField1).SelectedIndex = 0;
        }

        protected override void SetEntryField2FromRange(IMssMsgRange msgRange)
        {
            throw new NotImplementedException();
        }
    }
}

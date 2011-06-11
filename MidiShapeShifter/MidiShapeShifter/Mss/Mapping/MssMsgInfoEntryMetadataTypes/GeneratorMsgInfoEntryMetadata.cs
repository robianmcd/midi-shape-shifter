﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using MidiShapeShifter.Mss.Mapping.MssMsgInfoTypes;

namespace MidiShapeShifter.Mss.Mapping.MssMsgInfoEntryMetadataTypes
{
    public class GeneratorMsgInfoEntryMetadata : MssMsgInfoEntryMetadata
    {
        protected override Control EntryField1
        {
            get
            {
                if (this.ioCatagory == IoType.Input)
                {
                    return this.mappingDlg.inEntryField1Combo;
                }
                else
                {
                    //Cannot output to generator
                    Debug.Assert(false);
                    return null;
                }
            }
        }

        protected override Control EntryField2
        {
            get
            {
                return null;
            }
        }

        protected override void SetMappingDlgEntryFieldCustomProperties()
        {
            this.EntryField1Lbl.Visible = true;
            this.EntryField1Lbl.Text = "Generator Name:";

            this.EntryField1.Visible = true;
        }

        protected override MssMsgInfo CreateMsgInfoFromStoredContent()
        {
            GeneratorMsgInfo generatorMsgInfo = new GeneratorMsgInfo();
            //TODO: Initialize generatorMsgInfo
            return generatorMsgInfo;
        }

        protected override void InitSameAsInputCompatibleTypes()
        {
            
        }
    }
}

﻿//  Copyright 2016 Google Inc. All Rights Reserved.
//
//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
//
//  http://www.apache.org/licenses/LICENSE-2.0
//
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations under the License.

namespace NtApiDotNet
{
#pragma warning disable 1591
    /// <summary>
    /// Class representing various process mitigations
    /// </summary>
    public class NtProcessMitigations
    {
        internal NtProcessMitigations(NtProcess process)
        {
            ProcessDepStatus dep_status = process.DepStatus;
            DisableAtlThunkEmulation = dep_status.DisableAtlThunkEmulation;
            DepEnabled = dep_status.Enabled;
            DepPermanent = dep_status.Permanent;
                    
            int result = process.GetProcessMitigationPolicy(ProcessMitigationPolicy.ASLR);
            EnableBottomUpRandomization = result.GetBit(0);
            EnableForceRelocateImages = result.GetBit(1);
            EnableHighEntropy = result.GetBit(2);
            DisallowStrippedImages = result.GetBit(3);

            DisallowWin32kSystemCalls = process.GetProcessMitigationPolicy(ProcessMitigationPolicy.SystemCallDisable).GetBit(0);
            result = process.GetProcessMitigationPolicy(ProcessMitigationPolicy.StrictHandleCheck);
            RaiseExceptionOnInvalidHandleReference = result.GetBit(0);
            HandleExceptionsPermanentlyEnabled = result.GetBit(1);

            result = process.GetProcessMitigationPolicy(ProcessMitigationPolicy.FontDisable);
            DisableNonSystemFonts = result.GetBit(0);
            AuditNonSystemFontLoading = result.GetBit(1);

            result = process.GetProcessMitigationPolicy(ProcessMitigationPolicy.DynamicCode);
            ProhibitDynamicCode = result.GetBit(0);
            AllowThreadOptOut = result.GetBit(1);
            AllowRemoteDowngrade = result.GetBit(2);
            DisableExtensionPoints = process.GetProcessMitigationPolicy(ProcessMitigationPolicy.ExtensionPointDisable).GetBit(0);

            result = process.GetProcessMitigationPolicy(ProcessMitigationPolicy.ControlFlowGuard);
            EnabledControlFlowGuard = result.GetBit(0);
            EnableExportSuppression = result.GetBit(1);
            ControlFlowGuardStrictMode = result.GetBit(2);

            result = process.GetProcessMitigationPolicy(ProcessMitigationPolicy.Signature);
            MicrosoftSignedOnly = result.GetBit(0);
            StoreSignedOnly = result.GetBit(1);
            SignedMitigationOptIn = result.GetBit(2);

            result = process.GetProcessMitigationPolicy(ProcessMitigationPolicy.ImageLoad);
            NoRemoteImages = result.GetBit(0);
            NoLowMandatoryLabelImages = result.GetBit(1);
            PreferSystem32Images = result.GetBit(2);

            result = process.GetProcessMitigationPolicy(ProcessMitigationPolicy.ReturnFlowGuard);
            EnabledReturnFlowGuard = result.GetBit(0);
            ReturnFlowGuardStrictMode = result.GetBit(1);
            IsChildProcessRestricted = process.IsChildProcessRestricted;
            using (var token = NtToken.OpenProcessToken(process, TokenAccessRights.Query, false))
            {
                IsRestricted = token.Result.Restricted;
                IsAppContainer = token.Result.AppContainer;
                IsLowPrivilegeAppContainer = token.Result.LowPrivilegeAppContainer;
                IntegrityLevel = token.Result.IntegrityLevel;
            }
            ProcessId = process.ProcessId;
            Name = process.Name;
            ImagePath = process.FullPath;
            CommandLine = process.CommandLine;
        }

        public int ProcessId { get; private set; }
        public string Name { get; private set; }
        public string ImagePath { get; private set; }
        public string CommandLine { get; private set; }
        public bool DisallowWin32kSystemCalls { get; private set; }
        public bool DepEnabled { get; private set; }
        public bool DisableAtlThunkEmulation { get; private set; }
        public bool DepPermanent { get; private set; }
        public bool EnableBottomUpRandomization { get; private set; }
        public bool EnableForceRelocateImages { get; private set; }
        public bool EnableHighEntropy { get; private set; }
        public bool DisallowStrippedImages { get; private set; }
        public bool RaiseExceptionOnInvalidHandleReference { get; private set; }
        public bool HandleExceptionsPermanentlyEnabled { get; private set; }
        public bool DisableNonSystemFonts { get; private set; }
        public bool AuditNonSystemFontLoading { get; private set; }
        public bool ProhibitDynamicCode { get; private set; }
        public bool DisableExtensionPoints { get; private set; }
        public bool EnabledControlFlowGuard { get; private set; }
        public bool EnableExportSuppression { get; private set; }
        public bool ControlFlowGuardStrictMode { get; private set; }
        public bool MicrosoftSignedOnly { get; private set; }
        public bool StoreSignedOnly { get; private set; }
        public bool SignedMitigationOptIn { get; private set; }
        public bool NoRemoteImages { get; private set; }
        public bool NoLowMandatoryLabelImages { get; private set; }
        public bool PreferSystem32Images { get; private set; }
        public bool AllowThreadOptOut { get; private set; }
        public bool AllowRemoteDowngrade { get; private set; }
        public bool EnabledReturnFlowGuard { get; private set; }
        public bool ReturnFlowGuardStrictMode { get; private set; }
        public bool IsChildProcessRestricted { get; private set; }
        public bool IsRestricted { get; private set; }
        public bool IsAppContainer { get; private set; }
        public bool IsLowPrivilegeAppContainer { get; private set; }
        public TokenIntegrityLevel IntegrityLevel { get; private set; }
    }
#pragma warning restore 1591
}

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Valve.VR
{
    using System;
    using UnityEngine;
    
    
    public partial class SteamVR_Actions
    {
        
        private static SteamVR_Input_ActionSet_default p__default;
        
        private static SteamVR_Input_ActionSet_editor p_editor;
        
        private static SteamVR_Input_ActionSet_flight p_flight;
        
        private static SteamVR_Input_ActionSet_EVA p_EVA;
        
        public static SteamVR_Input_ActionSet_default _default
        {
            get
            {
                return SteamVR_Actions.p__default.GetCopy<SteamVR_Input_ActionSet_default>();
            }
        }
        
        public static SteamVR_Input_ActionSet_editor editor
        {
            get
            {
                return SteamVR_Actions.p_editor.GetCopy<SteamVR_Input_ActionSet_editor>();
            }
        }
        
        public static SteamVR_Input_ActionSet_flight flight
        {
            get
            {
                return SteamVR_Actions.p_flight.GetCopy<SteamVR_Input_ActionSet_flight>();
            }
        }
        
        public static SteamVR_Input_ActionSet_EVA EVA
        {
            get
            {
                return SteamVR_Actions.p_EVA.GetCopy<SteamVR_Input_ActionSet_EVA>();
            }
        }
        
        private static void StartPreInitActionSets()
        {
            SteamVR_Actions.p__default = ((SteamVR_Input_ActionSet_default)(SteamVR_ActionSet.Create<SteamVR_Input_ActionSet_default>("/actions/default")));
            SteamVR_Actions.p_editor = ((SteamVR_Input_ActionSet_editor)(SteamVR_ActionSet.Create<SteamVR_Input_ActionSet_editor>("/actions/editor")));
            SteamVR_Actions.p_flight = ((SteamVR_Input_ActionSet_flight)(SteamVR_ActionSet.Create<SteamVR_Input_ActionSet_flight>("/actions/flight")));
            SteamVR_Actions.p_EVA = ((SteamVR_Input_ActionSet_EVA)(SteamVR_ActionSet.Create<SteamVR_Input_ActionSet_EVA>("/actions/EVA")));
            Valve.VR.SteamVR_Input.actionSets = new Valve.VR.SteamVR_ActionSet[] {
                    SteamVR_Actions._default,
                    SteamVR_Actions.editor,
                    SteamVR_Actions.flight,
                    SteamVR_Actions.EVA};
        }
    }
}

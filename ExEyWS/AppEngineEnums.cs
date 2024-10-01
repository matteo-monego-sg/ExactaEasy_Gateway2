using System;
using System.ServiceModel;

namespace ExactaEasyEng
{
    public enum GatewayCommunicationStatusEnum
    {
        NotCreated = -1,
        NotConnected,
        Connecting,
        Connected,
        Established
    }

    public enum HMIServiceStateEnum
    {
        NotCreated = -1,
        NotConnected = 0,
        Connecting,
        Connected
    }

    public enum SupervisorServiceStateEnum
    {

        NotCreated = -1,
        Created = CommunicationState.Created,
        Opening = CommunicationState.Opening,
        Opened = CommunicationState.Opened,
        Closing = CommunicationState.Closing,
        Closed = CommunicationState.Closed,
        Faulted = CommunicationState.Faulted
    }

    public enum SupervisorModeEnum
    {
        Unknown = 0,
        ReadyToRun = 1,
        Editing = 2,
        Error = 3,
        Busy = 4,
        WaitForKnappStart = 5,
        ApplicationStarted = 100
    }

    public enum SaveModeEnum
    {
        DontSave = -1,
        Unknown = 0,
        Setup = 1,
        Production = 2
    }

    public enum MachineModeEnum
    {

        Unknown = 0,
        Stop = 1,
        Running = 2,
        Error = 3,
        //ReadyToRun = 8,
        //Editing = 16        
    }

    public enum UserLevelEnum
    {

        None = 0,
        Operator = 1,
        AssistantSupervisor = 4,
        Supervisor = 5,
        Engineer = 7,
        MinorAdministrator = 8, // Same as Administrator, but without the possibility to save/apply parameters
        Administrator = 9,
        Optrel = 10
    }

    public enum SupervisorWorkingModeEnum
    {

        Unknown = 0,
        Normal,
        Knapp,
        Manual
    }


    // Moved to ExEyGateway.SharedEnums.cs to maintain back-compatibility with ARTIC versions.
    //public enum RecipeStatusEnum
    //{
    //    Unknown = 0,
    //    Valid = 1,
    //    RCandidate = 5,
    //    Deleted = 7,
    //    Neutral = 100,
    //}

    [Flags]
    public enum ContextChangesEnum
    {
        Position = 0x01,
        Language = 0x02,
        UserLevel = 0x04,
        ActiveRecipe = 0x08,
        ActiveRecipeVersion = 0x9,
        ActiveRecipeStatus = 0x77,
        MachineMode = 0x10,
        MachineInfo = 0x20,
        SupervisorMode = 0x40,
        SupervisorWorkingMode = 0x80,
        Batch = 0x100,
        DataBase = 0x200,
        All = 0xFFF
    }
}

﻿namespace Dalamud.DrunkenToad.Core.Services;

using System;
using System.Linq;
using Dalamud.Game.ClientState.Objects;
using FFXIVClientStructs.FFXIV.Client.Game.Object;
using FFXIVClientStructs.FFXIV.Client.UI.Agent;

/// <summary>
/// Target manager wrapper to provide additional functionality.
/// https://github.com/goatcorp/Dalamud/blob/master/Dalamud/Game/ClientState/Objects/TargetManager.cs.
/// </summary>
public class TargetManagerEx
{
    private readonly ITargetManager targetManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="TargetManagerEx" /> class.
    /// </summary>
    /// <param name="targetManager">dalamud target manager.</param>
    public TargetManagerEx(ITargetManager targetManager) => this.targetManager = targetManager;

    /// <summary>
    /// Sets the target to the specified object ID. If the object ID is already targeted, it will clear the target.
    /// </summary>
    /// <param name="objectId">The object ID to target.</param>
    public void SetTarget(uint objectId)
    {
        var obj = DalamudContext.ObjectCollection.SearchById(objectId);
        if (obj == null)
        {
            return;
        }

        if (this.targetManager.Target?.EntityId == obj.EntityId)
        {
            this.targetManager.Target = null;
            return;
        }

        this.targetManager.Target = obj;
    }

    /// <summary>
    /// Sets the focus target to the specified object ID. If the object ID is already focused, it will clear the focus
    /// target.
    /// </summary>
    /// <param name="objectId">The object ID to set as focus target.</param>
    public void SetFocusTarget(uint objectId)
    {
        var obj = DalamudContext.ObjectCollection.SearchById(objectId);
        if (obj == null)
        {
            return;
        }

        if (this.targetManager.FocusTarget?.EntityId == obj.EntityId)
        {
            this.targetManager.FocusTarget = null;
            return;
        }

        this.targetManager.FocusTarget = obj;
    }

    /// <summary>
    /// Opens the plate window for the specified object ID.
    /// </summary>
    /// <param name="objectId">The object ID for which to open the plate window.</param>
    public unsafe void OpenPlateWindow(uint objectId)
    {
        var obj = DalamudContext.ObjectCollection.FirstOrDefault(i => i.EntityId == objectId);
        if (obj == null)
        {
            return;
        }

        try
        {
            AgentCharaCard.Instance()->OpenCharaCard((GameObject*)obj.Address);
        }
        catch (Exception ex)
        {
            DalamudContext.PluginLog.Error(ex, "Failed to open plate window.");
        }
    }

    /// <summary>
    /// Examines the specified object ID.
    /// </summary>
    /// <param name="objectId">The object ID to examine.</param>
    public unsafe void ExamineTarget(uint objectId)
    {
        var obj = DalamudContext.ObjectCollection.FirstOrDefault(i => i.EntityId == objectId);
        if (obj == null)
        {
            return;
        }

        try
        {
            AgentInspect.Instance()->ExamineCharacter(objectId);
        }
        catch (Exception ex)
        {
            DalamudContext.PluginLog.Error(ex, "Failed to examine target.");
        }
    }
}

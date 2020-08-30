﻿using UnityEngine;
using System.Collections.Generic;

namespace PixelCrushers.DialogueSystem
{

    /// <summary>
    /// Adds three Lua functions:
    /// - PushConversationPosition(): Saves the current conversation position on the top of a stack.
    /// - PopConversationPosition(): Removes the conversation position on the top of the stack and returns to it.
    /// - ClearConversationPositionStack(): Clears the stack.
    /// </summary>
    [AddComponentMenu("")] // Use wrapper.
    public class ConversationPositionStack : MonoBehaviour
    {
        [Tooltip("Clear stack when new conversation starts. Only applies if component is on Dialogue Manager.")]
        public bool clearOnConversationStart = true;

        [Tooltip("Typically leave unticked so temporary Dialogue Manager's don't unregister your functions.")]
        public bool unregisterOnDisable = false;

        private static Stack<DialogueEntry> m_stack = new Stack<DialogueEntry>();

#if UNITY_2019_3_OR_NEWER
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void InitStaticVariables()
        {
            m_stack = new Stack<DialogueEntry>();
        }
#endif

        private void OnEnable()
        {
            Lua.RegisterFunction("PushConversationPosition", null, SymbolExtensions.GetMethodInfo(() => PushConversationPosition()));
            Lua.RegisterFunction("PopConversationPosition", null, SymbolExtensions.GetMethodInfo(() => PopConversationPosition()));
            Lua.RegisterFunction("ClearConversationPositionStack", null, SymbolExtensions.GetMethodInfo(() => ClearConversationPositionStack()));
        }

        private void OnDisable()
        {
            if (unregisterOnDisable)
            {
                Lua.UnregisterFunction("PushConversationPosition");
                Lua.UnregisterFunction("PopConversationPosition");
                Lua.UnregisterFunction("ClearConversationPositionStack");
            }
        }

        private void OnConversationStart(Transform actor)
        {
            if (clearOnConversationStart) ClearConversationPositionStack();
        }

        public static void PushConversationPosition()
        {
            try
            {
                if (!DialogueManager.isConversationActive || DialogueManager.currentConversationState == null)
                {
                    if (DialogueDebug.logWarnings) Debug.LogWarning("Dialogue System: PushConversationPosition() Lua function can't save the current conversation position because no conversation is active.");
                }
                else
                {

                    var state = DialogueManager.currentConversationState;
                    var entry = state.hasNPCResponse ? state.firstNPCResponse.destinationEntry
                            : state.hasPCResponses ? state.pcResponses[0].destinationEntry : null;
                    if (entry == null)
                    {
                        if (DialogueDebug.logWarnings) Debug.LogWarning("Dialogue System: PushConversationPosition() Lua function can't save the current conversation position because it's invalid.");
                    }
                    else
                    {
                        m_stack.Push(entry);
                        if (DialogueDebug.logInfo) Debug.Log("Dialogue System: PushConversationPosition() Lua function saved entry " + entry.conversationID + ":" + entry.id + ": '" + entry.currentDialogueText + "'.");
                    }
                }
            }
            catch (System.Exception e)
            {
                Debug.LogException(e);
                throw e;
            }
        }

        public static void PopConversationPosition()
        {
            try
            {
                if (!DialogueManager.isConversationActive || DialogueManager.currentConversationState == null)
                {
                    if (DialogueDebug.logWarnings) Debug.LogWarning("Dialogue System: PopConversationPosition() Lua function can't restore a saved conversation position because no conversation is active.");
                }
                else if (m_stack.Count == 0)
                {
                    if (DialogueDebug.logWarnings) Debug.LogWarning("Dialogue System: PopConversationPosition() Lua function can't restore a saved conversation position no positions are saved on the stack.");
                }
                else
                {
                    var entry = m_stack.Pop();
                    if (DialogueDebug.logInfo) Debug.Log("Dialogue System: PopConversationPosition() Lua function is returning to entry " + entry.conversationID + ":" + entry.id + ": '" + entry.currentDialogueText + "'.");
                    DialogueManager.conversationModel.ForceNextStateToLinkToEntry(entry);
                }
            }
            catch (System.Exception e)
            {
                Debug.LogException(e);
                throw e;
            }
        }

        public static void ClearConversationPositionStack()
        {
            if (DialogueDebug.logInfo) Debug.Log("Dialogue System: ClearConversationPosition() Lua function is clearing the conversation position stack.");
            m_stack.Clear();
        }
    }
}

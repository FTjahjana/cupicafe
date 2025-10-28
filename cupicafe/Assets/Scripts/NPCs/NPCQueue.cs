using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCQueue : MonoBehaviour
{
    [System.Serializable]
    public class Slot
    {
        public Transform slotLoc;
        public NPCBehaviour occupiedBy;
        public bool occupied;
    }

    public List<Slot> slots = new List<Slot>();

    public bool AssignSlot(NPCBehaviour npc)
    {
        foreach (var slot in slots)
        {
            if (!slot.occupied && slot.slotLoc != null)
            {
                slot.occupied = true;
                npc.transform.position = slot.slotLoc.position + npc.offset;
                npc.transform.rotation = slot.slotLoc.rotation;
                npc.Animator.Play(npc.sitAnimation);
                return true;
            }
        }
        return false;
    }

    public void ReleaseSlot(Transform npcTransform)
    {
        foreach (var slot in slots)
        {
            if (slot.slotLoc != null && Vector3.Distance(npcTransform.position, slot.slotLoc.position) < 0.1f)
            {
                slot.occupied = false;
                break;
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Gorun Gork/NPC/Dialogue NPC")]
public class DialogueNPC : NPCTemplate {
	public Color color = Color.white;
	[Space(10)]
	public Dialogue dialogueTree;
}

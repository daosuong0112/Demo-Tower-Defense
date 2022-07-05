using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITargetSkill {

	Transform GetTransform { get; }
	ViewType viewType { get; }
	int Id { get; }
	TeamType TeamType { get; }
}

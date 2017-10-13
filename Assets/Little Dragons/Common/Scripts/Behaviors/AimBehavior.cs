using UnityEngine;

public class AimBehavior : StateMachineBehaviour
{
    public bool active;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        DragoFire DF = animator.GetComponent<DragoFire>();
        if (DF) DF.Activate(active);

    }
}

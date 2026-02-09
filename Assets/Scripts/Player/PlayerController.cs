using Fusion;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    private NetworkCharacterController _controller;

    private void Awake()
    {
        _controller = GetComponent<NetworkCharacterController>();
    }

    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();

        if(GetInput(out NetworkInputData data))
        {
            data.direction.Normalize();
            _controller.Move(5 * data.direction * Runner.DeltaTime);

            Vector3 lookDir = data.mousePosition - transform.position;
            lookDir.y = 0;
            if(lookDir.sqrMagnitude > 0.1)
            {
                transform.rotation = Quaternion.LookRotation(lookDir);
            }
        }
    }
}

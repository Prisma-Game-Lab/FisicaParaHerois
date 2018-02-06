using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Boss))]
[RequireComponent(typeof(ChangeGravityAction))]
[RequireComponent(typeof(PushPullAction))]
public class BossAttackState : MonoBehaviour, IState
{
    
    [HideInInspector]public Boss StateMachineController;


    [Header("Game Design Area")]
    public float upGravity;
    public float downGravity;
    public float lateralVelocity;
    public float delay;

    [Header("Unity Stuff")]
    public GameObject boxPrefab;
    public Transform spawnPosition;
    public Transform boxDesiredHeight;

    private List<GameObject> _spawnedBoxes;

    private ChangeGravityAction _cga;

    private GameObject _player;

    public void Awake()
    {
        StateMachineController = GetComponent<Boss>();
        _spawnedBoxes = new List<GameObject>();
        _cga = GetComponent<ChangeGravityAction>();
        _player = FindObjectOfType<PlayerInput>().gameObject;
    }

    public void ResumeState()
    {
        this.StartState();
    }

    public void StartState()
    {
        //instancia uma nova caixa na posição de spawn position
        _spawnedBoxes.Add(Instantiate(boxPrefab, spawnPosition.transform.position, Quaternion.identity));
        StateMachineController.OnBoxSpawn(_spawnedBoxes[_spawnedBoxes.Count - 1]);
        StartCoroutine("AttackSequence", _spawnedBoxes[_spawnedBoxes.Count - 1]);

    }

    public void StopState()
    {

    }

    public void UpdateState()
    {
              
        
    }

    //implementa diversas etapas de um ataque básico, usando a caixa instanciada
    IEnumerator AttackSequence(GameObject box)
    {
        PhysicsObject po = box.GetComponent<PhysicsObject>();
        po.CanPlayerInteract = false;
        //talvez adicionar um timer, se passar de tantos segundos || a caixa ficar parada tantos segundos, destruir a caixa

        //movimenta a caixa até a altura certa

        //se eu estou manipulando mais de uma caixa ao mesmo tempo, isso vai dar problema?
        //por essas e outras que eu acho que seria melhor ter no obj o componente cga, e não em quem age
        //solução se der problema: instanciar um novo componente _cga, de maneira que haverá um só para manipular cada caixa

        //obs: eu to usando as actions, mas ainda assim posso alterar diretamente os valores que as actions alteram. tem algo estranho nesse encapsulamento

        _cga.SetTarget(po);

        //sobe
        _cga.OnActionUse(upGravity);
        yield return new WaitWhile(() => box.transform.position.y < boxDesiredHeight.position.y);

        //desacelera
        _cga.OnActionUse(-2*upGravity);
        yield return new WaitUntil(() => (po.physicsData.velocity.y < 0.1f));

        //pára
        _cga.OnActionUse(0.0f);
        po.physicsData.velocity = Vector2.zero;

        yield return new WaitForSeconds(delay);

        //move na direção do player
        po.physicsData.isKinematic = true;
        int direction = (_player.transform.position.x > box.transform.position.x) ? 1 : -1;

        po.physicsData.velocity = new Vector2(lateralVelocity * direction, 0.0f);

        yield return new WaitUntil(() => ((_player.transform.position.x - box.transform.position.x) * direction < 0));

        //pára
        po.physicsData.velocity = Vector2.zero;
        po.physicsData.isKinematic = false;

        yield return new WaitForSeconds(delay);

        //cai
        _cga.OnActionUse(downGravity);

        po.CanPlayerInteract = true;

        //troca de estado e termina ação
        StateMachineController.ChangeState(StateMachineController.IdleState);
        yield return null;






    }



    
}
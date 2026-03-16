using UnityEngine;

public class Focus_pistolero : State_Base
{
    private Animation_Controller anim;
    private float timer = 1f;
    private GameObject brazoPistola;
    private GameObject manoConPistola;
    private ManagerMarineros marineroManager;
    public GameObject proyectilPrefab;

    public override void EnterState()
    {
        anim = controlledObject.GetComponent<Animation_Controller>();
        marineroManager = controlledObject.GetComponent<ManagerMarineros>();

        // Activar animacion Focus (indice 1)
        if (anim != null)
            anim.Play(1);

        brazoPistola = FindChildWithName(controlledObject.transform, "brazo con pistola");
        if (brazoPistola != null)
            brazoPistola.SetActive(true);

        manoConPistola = brazoPistola;

        timer = 1f;
    }

    public override void UpdateState()
    {
        if (anim != null)
            anim.Play(1);

        timer -= Time.deltaTime;

        ApuntarAlEnemigo();

        if (timer <= 0f)
        {
            ExitState("Charge_pistolero");
        }
    }

    private void ApuntarAlEnemigo()
    {
        if (marineroManager == null || manoConPistola == null)
            return;

        GameObject enemigo = marineroManager.FindClosestEnemy();
        if (enemigo == null)
            return;

        Vector3 centroHombro =
            controlledObject.transform.position + new Vector3(0.2f, 0.5f, 0);

        Vector3 direccion = enemigo.transform.position - centroHombro;

        float anguloRad = Mathf.Atan2(direccion.y, direccion.x);
        float anguloDeg = anguloRad * Mathf.Rad2Deg;

        float radio = 0.8f;
        Vector3 offsetPosicion =
            new Vector3(Mathf.Cos(anguloRad), Mathf.Sin(anguloRad), 0) * radio;

        manoConPistola.transform.position = centroHombro + offsetPosicion;
        manoConPistola.transform.rotation =
            Quaternion.Euler(0, 0, anguloDeg - 10f);

        SpriteRenderer sr = manoConPistola.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            bool miraIzquierda = Mathf.Abs(anguloDeg) > 90f;
            sr.flipY = miraIzquierda;
            sr.sortingOrder = miraIzquierda ? -1 : 1;
        }
    }

    private GameObject FindChildWithName(Transform parent, string name)
    {
        foreach (Transform child in parent)
        {
            if (child.name == name)
                return child.gameObject;

            GameObject found = FindChildWithName(child, name);
            if (found != null)
                return found;
        }
        return null;
    }

    public override void ExitState(string nextState)
    {
        if (brazoPistola != null && nextState != "disparando")
            brazoPistola.SetActive(false);

        if (nextState == "Charge_pistolero")
        {
            if (proyectilPrefab != null && marineroManager != null)
            {
                GameObject enemigo = marineroManager.FindClosestEnemy();

                if (enemigo != null)
                {
                    Vector3 posicionInstancia =
                        brazoPistola != null ?
                        brazoPistola.transform.position :
                        controlledObject.transform.position;

                    GameObject proyectilObj =
                        Instantiate(proyectilPrefab, posicionInstancia, Quaternion.identity);

                    proyectilObj.transform.localScale =
                        new Vector3(0.5f, 0.5f, 1f);

                    Bullet bulletScript = proyectilObj.GetComponent<Bullet>();

                    if (bulletScript != null)
                    {
                        int power = 1;
                        float speed = 20f;
                        int pierce = 0;

                        bulletScript.Initialize(
                            enemigo.transform.position,
                            speed,
                            power,
                            pierce
                        );

                        bulletScript.LaunchTowards(
                            enemigo.transform,
                            speed
                        );
                    }
                }
            }

            state_machine.SetState<Charge_pistolero>();
        }
    }
}
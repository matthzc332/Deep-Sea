using UnityEngine;

public class Focus_pistolero : State_Base
{
    private Animation_Controller anim;
    private float timer = 1f;
    private GameObject brazoPistola;
    private ManagerMarineros marineroManager;
    public GameObject proyectilPrefab;

    public override void EnterState()
    {
        anim = controlledObject.GetComponent<Animation_Controller>();
        marineroManager = controlledObject.GetComponent<ManagerMarineros>();

        if (anim != null)
            anim.Play(1);

        // Buscamos el objeto con el nuevo nombre
        brazoPistola = FindChildWithName(controlledObject.transform, "brazo con pistola");

        if (brazoPistola != null)
            brazoPistola.SetActive(true);

        timer = 1f;
    }

    public override void UpdateState()
    {
        timer -= Time.deltaTime;

        ApuntarAlEnemigo();

        if (timer <= 0f)
        {
            ExitState("Charge_pistolero");
        }
    }

    private void ApuntarAlEnemigo()
    {
        if (marineroManager == null || brazoPistola == null)
            return;

        GameObject enemigo = marineroManager.FindClosestEnemy();
        if (enemigo == null)
            return;

        // 1. Obtener la dirección hacia el enemigo
        Vector3 direccion = enemigo.transform.position - brazoPistola.transform.position;

        // 2. Calcular el ángulo en grados
        float anguloDeg = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg;

        // 3. Detectar si el entorno (padre/posición) está invertido
        // Usamos lossyScale para saber la escala REAL final del objeto en el mundo
        bool estaInvertido = brazoPistola.transform.lossyScale.x < 0;

        if (estaInvertido)
        {
            // Si el mundo está invertido, sumamos 180 grados para compensar 
            // que el eje X local apunta hacia el otro lado
            brazoPistola.transform.rotation = Quaternion.Euler(0, 0, anguloDeg + 180f);
        }
        else
        {
            brazoPistola.transform.rotation = Quaternion.Euler(0, 0, anguloDeg);
        }

        // 4. Ajustar el Flip del Sprite para que la pistola no quede boca abajo
        SpriteRenderer sr = brazoPistola.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            // Si el ángulo absoluto es mayor a 90, el brazo apunta "atrás" respecto a su origen
            bool apuntandoHaciaAtras = Mathf.Abs(anguloDeg) > 90f;

            // Invertimos el eje Y del sprite dependiendo de si el padre ya está invertido o no
            sr.flipY = estaInvertido ? !apuntandoHaciaAtras : apuntandoHaciaAtras;
        }
    }

    // El resto de tus métodos (FindChildWithName y ExitState) se mantienen igual, 
    // pero usando la posición real de brazoPistola para el Instantiate.

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
                    // La bala sale del brazo (puedes sumarle un pequeño offset si quieres que salga de la punta)
                    Vector3 posicionInstancia = brazoPistola.transform.position;

                    GameObject proyectilObj = Instantiate(proyectilPrefab, posicionInstancia, Quaternion.identity);
                    proyectilObj.transform.localScale = new Vector3(0.5f, 0.5f, 1f);

                    Bullet bulletScript = proyectilObj.GetComponent<Bullet>();
                    if (bulletScript != null)
                    {
                        float speed = 20f;
                        bulletScript.Initialize(enemigo.transform.position, speed, 1, 0);
                        bulletScript.LaunchTowards(enemigo.transform, speed);
                    }
                }
            }
            state_machine.SetState<Charge_pistolero>();
        }
    }

    private GameObject FindChildWithName(Transform parent, string name)
    {
        foreach (Transform child in parent)
        {
            if (child.name == name) return child.gameObject;
            GameObject found = FindChildWithName(child, name);
            if (found != null) return found;
        }
        return null;
    }
}
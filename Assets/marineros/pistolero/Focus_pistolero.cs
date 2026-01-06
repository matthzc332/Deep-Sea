using UnityEngine;

public class Focus_pistolero : State_Base
{
    private Animator animator;
    private float timer = 1f;
    private GameObject brazoPistola;
    private ManagerMarineros marineroManager;
    public GameObject proyectilPrefab;

    public override void EnterState(){
        // Obtener componentes
        animator = controlledObject.GetComponent<Animator>();
        marineroManager = controlledObject.GetComponent<ManagerMarineros>();
        
        // Reproducir la animación "apuntando"
        if (animator != null)
        {
            animator.Play("apuntando");
        }
        else
        {
            Debug.LogWarning("Animator no encontrado en el objeto controlado: " + controlledObject.name);
        }
        
        // Buscar y activar el brazo con pistola
        brazoPistola = FindChildWithName(controlledObject.transform, "brazo con pistola");
        if (brazoPistola != null)
        {
            brazoPistola.SetActive(true);
        }
        else
        {
            Debug.LogWarning("No se encontró el hijo 'brazo con pistola'");
        }
        
        // Reiniciar el timer
        timer = 1f;
    }

    public override void UpdateState(){
        // Reducir el timer
        timer -= Time.deltaTime;
        
        // Apuntar hacia el enemigo más cercano
        ApuntarAlEnemigo();
        
        // Verificar si el timer llegó a 0 para cambiar de estado (ejemplo)
        if (timer <= 0f)
        {
            // Cambiar al siguiente estado (disparar, por ejemplo)
            ExitState("Charge_pistolero");

        }
    }

    private void ApuntarAlEnemigo()
    {
        if (marineroManager != null && brazoPistola != null)
        {
            GameObject enemigoCercano = marineroManager.FindClosestEnemy();
            
            if (enemigoCercano != null)
            {
                // Calcular dirección hacia el enemigo
                Vector3 direccion = enemigoCercano.transform.position - brazoPistola.transform.position;
                
                // Calcular ángulo de rotación
                float angulo = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg;
                angulo = angulo-10f;
                
                // Aplicar rotación al brazo
                brazoPistola.transform.rotation = Quaternion.Euler(0f, 0f, angulo);
                
            }
        }
    }


    // Función auxiliar para buscar hijo por nombre
    private GameObject FindChildWithName(Transform parent, string name)
    {
        foreach (Transform child in parent)
        {
            if (child.name == name)
                return child.gameObject;
            
            // Búsqueda recursiva en hijos
            GameObject found = FindChildWithName(child, name);
            if (found != null)
                return found;
        }
        return null;
    }





    public override void ExitState(string nextState)
    {
        // Desactivar el brazo al salir del estado si es necesario
        if (brazoPistola != null && nextState != "disparando")
        {
            brazoPistola.SetActive(false);
        }
        
        if (nextState == "Charge_pistolero")
        {
            if (proyectilPrefab != null)
            {
                GameObject enemigoCercano = marineroManager.FindClosestEnemy();
                
                if (enemigoCercano != null)
                {
                    Vector3 posicionInstancia = brazoPistola != null ? 
                        brazoPistola.transform.position : 
                        controlledObject.transform.position;
                    
                    GameObject proyectilObj = Instantiate(proyectilPrefab, posicionInstancia, Quaternion.identity);
                    proyectilObj.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
                    Bullet bulletScript = proyectilObj.GetComponent<Bullet>();
                    
                    if (bulletScript != null)
                    {
                        int power = 1; 
                        float speed = 20f; 
                        int pierce = 0; // Añadimos el parámetro que faltaba

                        // ERROR 1 CORREGIDO: Ahora enviamos los 4 parámetros (Vector3, float, int, int)
                        bulletScript.Initialize(enemigoCercano.transform.position, speed, power, pierce);
                        
                        // ERROR 2 CORREGIDO: 
                        // Si quieres usar LaunchTowards con un Transform (el enemigo), 
                        // debes pasar el objeto completo, no solo su .position
                        bulletScript.LaunchTowards(enemigoCercano.transform, speed);
                    }
                    else
                    {
                        Debug.LogError("El prefab Proyectil no tiene el componente Bullet");
                    }
                }
            }
            
            // Cambiar al siguiente estado
            state_machine.SetState<Charge_pistolero>();
        }
    }
}
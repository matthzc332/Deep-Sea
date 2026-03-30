// using UnityEngine;
// using UnityEngine.SceneManagement;
// using System.Collections;
// using DG.Tweening;
// public class Ship : Entity
// {
//     private bool esInvulnerable = false; // Controla si puede recibir daño
//     private Transform Ships;
//     private SpriteRenderer spr;
//     public Joystick joystick;

//     public float vida;

//     // Límites de movimiento
//     private float limiteDerecho = 7.300274f;
//     private float limiteIzquierdo = -7.300274f;
//     public float initialSpeed = 0.9f;

//     public ShipData shipData;
//     public Transform puntoPosicion1;
//     public Transform puntoPosicion2;


//    void Start()
//     {
//         Ships = GetComponent<Transform>();
//         spr = gameObject.GetComponent<SpriteRenderer>();
//         speed = initialSpeed;

//         if (shipData != null)
//         {
//             // Sincronizar vida
//             HP = shipData.puntosDeVida;

//             // INSTANCIAR MARINEROS EN POSICIONES FIJAS
//             CargarTripulacionFija();
//         }
//     }

//     private void CargarTripulacionFija()
//     {
//         // Posición 1
//         if (shipData.posicion1 != null && puntoPosicion1 != null)
//         {
//             Instantiate(shipData.posicion1, puntoPosicion1.position, Quaternion.identity, puntoPosicion1);
//             Debug.Log("Marinero instanciado en Posición 1");
//         }

//         // Posición 2
//         if (shipData.posicion2 != null && puntoPosicion2 != null)
//         {
//             Instantiate(shipData.posicion2, puntoPosicion2.position, Quaternion.identity, puntoPosicion2);
//             Debug.Log("Marinero instanciado en Posición 2");
//         }
//     }
    
//     void Update()
//     {
//         // Solo mover si el joystick está activo y está dentro de los límites
//         if (joystick.angulo != 0f)
//         {
//             splitSpeed();
            
//             // Verificar límites específicos para cada dirección
//             if ((speed > 0 && Ships.position.x < limiteDerecho) || 
//                 (speed < 0 && Ships.position.x > limiteIzquierdo))
//             {
//                 move(speed, Ships);
//             }
//         }

//         vida = HP;
//     }

//     void move(float speed, Transform body)
//     {
//         body.Translate(Vector3.right * speed * Time.deltaTime);
        
//         // Aplicar límites después del movimiento
//         Vector3 pos = body.position;
//         pos.x = Mathf.Clamp(pos.x, limiteIzquierdo, limiteDerecho);
//         body.position = pos;
//     }
    
//     public void splitSpeed()
//     {
//         if (joystick.angulo > 0)
//         {
//             speed = initialSpeed*-1; // Mover hacia la izquierda
//         }
//         else if (joystick.angulo < 0)
//         {
//             speed = initialSpeed; // Mover hacia la derecha
//         }
//     }

//     protected virtual void OnTriggerEnter2D(Collider2D other)
//         {
//             base.OnTriggerEnter2D(other); 

//             if (other.CompareTag("Enemy"))
//             {
//             // Debug.Log("Trigger con: " + other.name + " (tag: " + other.tag + ")");
//                 if (other.CompareTag("Enemy"))
//                 {
//                     //Debug.Log("colisionó un enemigo");
//                 }
//             }
//         }
// // Usando DOTween para animar la colicion con el barco
//     private void OnTriggerEnter2D(Collider other)
//     {
//         var sequence = DOTween.Sequence();
//         sequence.Insert(atPosition: 0, t:_spr.DOColor(Color.red, DurationUnit: 0.1f));
//         sequence.Insert(atPosition: 0, t:Camera.main.DOShakePosition(duration:0.2f, strength:0.1f));
//         sequence.Insert(atPosition: 0.1f, t:_spr.DOColor(Color.white, DurationUnit: 0.1f));
//         Ships.ApplyDamage();
//     }

//     // public override void takeDamage(int damage)
//     // {
//     //     if (isAlive)
//     //     {
//     //         HP -= damage;
//     //         if (HP <= 0)
//     //         {
//     //             isAlive = false;
//     //             SceneManager.LoadScene("prefabs");
//     //         }
//     //     }
//     // }

//     // modifico take damage para no morir de ataques constantes del jefe
//     public override void takeDamage(int damage)
//     {
//         // 1. Si ya murió o es invulnerable, no hacemos nada
//         if (!isAlive || esInvulnerable) return;

//         // 2. Aplicamos daño
//         HP -= damage;
        
//         // Debug para ver que no baje a lo loco
//         Debug.Log($"Barco golpeado. Vida restante: {HP}");

//         if (HP <= 0)
//         {
//             isAlive = false;
//             SceneManager.LoadScene("prefabs");
//         }
//         else
//         {
//             Debug.Log("Algo golpeo invicibilidad, vida actual: " + HP);
//             // 3. Si sigue vivo, activamos la invulnerabilidad temporal
//             StartCoroutine(RutinaInvulnerabilidad());
//         }
//     }
//     // CORRUTINA NUEVA PARA EL TIEMPO DE GRACIA
//     IEnumerator RutinaInvulnerabilidad()
//     {
//         esInvulnerable = true;
        
//         // Opcional: Hacemos que el barco parpadee (se ponga medio transparente)
//         if (spr != null) spr.color = new Color(1f, 1f, 1f, 0.5f);

//         // Esperamos 1.5 segundos (puedes cambiar este número)
//         yield return new WaitForSeconds(1.5f);

//         // Volvemos a la normalidad
//         if (spr != null) spr.color = Color.white;
//         esInvulnerable = false;
        
//         Debug.Log("Barco vulnerable de nuevo.");
//     }

// }
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using DG.Tweening;

public class Ship : Entity
{
    private bool esInvulnerable = false; // Controla si puede recibir daño
    private Transform Ships;
    private SpriteRenderer spr;
    public Joystick joystick;

    public float vida;

    [Header("UI Visuals")]
    public SingleSeaWaveUI barraVidaOndulante;
    private float hpMaximo; // Para calcular el porcentaje

    [Header("Movimiento")]
    // Límites de movimiento
    private float limiteDerecho = 7.300274f;
    private float limiteIzquierdo = -7.300274f;
    public float initialSpeed = 0.9f;

    [Header("Posiciones")]
    
    public Transform puntoPosicion1;
    public Transform puntoPosicion2;
    //efectos de audio barco
    [Header("Otros")]
    public ShipData shipData;
    public ShipSound shipSound;

    void Start()
    {
        Ships = GetComponent<Transform>();
        spr = gameObject.GetComponent<SpriteRenderer>();
        speed = initialSpeed;

        if (shipData != null)
        {
            // Sincronizar vida
            HP = shipData.puntosDeVida;
            hpMaximo = 6;

            // INSTANCIAR MARINEROS EN POSICIONES FIJAS
            CargarTripulacionFija();
            // Inicializamos la barra al 100%
            ActualizarVisualVida();
        }
    }

    private void CargarTripulacionFija()
    {
        // Posición 1
        if (shipData.posicion1 != null && puntoPosicion1 != null)
        {
            Instantiate(shipData.posicion1, puntoPosicion1.position, Quaternion.identity, puntoPosicion1);
            Debug.Log("Marinero instanciado en Posición 1");
        }

        // Posición 2
        if (shipData.posicion2 != null && puntoPosicion2 != null)
        {
            Instantiate(shipData.posicion2, puntoPosicion2.position, Quaternion.identity, puntoPosicion2);
            Debug.Log("Marinero instanciado en Posición 2");
        }
    }

    void Update()
    {
        // 1. Verificación de nulidad: Si no hay joystick asignado, ignoramos el movimiento.
        if (joystick == null)
        {
            vida = HP; // Actualizamos la vida de todos modos para que la UI no se rompa
            return;
        }

        // 2. Solo mover si el joystick está activo y está dentro de los límites
        if (joystick.angulo != 0f)
        {
            splitSpeed();

            // Verificar límites específicos para cada dirección
            if ((speed > 0 && Ships.position.x < limiteDerecho) ||
                (speed < 0 && Ships.position.x > limiteIzquierdo))
            {
                move(speed, Ships);
            }
        }

        vida = HP;
    }

    void move(float speed, Transform body)
    {
        body.Translate(Vector3.right * speed * Time.deltaTime);
        
        // Aplicar límites después del movimiento
        Vector3 pos = body.position;
        pos.x = Mathf.Clamp(pos.x, limiteIzquierdo, limiteDerecho);
        body.position = pos;
    }
    
    // public void splitSpeed()
    // {
    //     if (joystick.angulo > 0)
    //     {
    //         speed = initialSpeed*-1; // Mover hacia la izquierda
    //     }
    //     else if (joystick.angulo < 0)
    //     {
    //         speed = initialSpeed; // Mover hacia la derecha
    //     }
    // }
    private float ultimaDireccion; // Para detectar cambios

public void splitSpeed()
{
    float nuevaVelocidad = 0;

    if (joystick.angulo > 0)
    {
        nuevaVelocidad = initialSpeed * -1; // Izquierda
    }
    else if (joystick.angulo < 0)
    {
        nuevaVelocidad = initialSpeed; // Derecha
    }

    // Si la nueva velocidad es distinta a la anterior, el timón se movió
    if (nuevaVelocidad != speed && nuevaVelocidad != 0)
    {
        shipSound.Timon();
    }

    speed = nuevaVelocidad;
}

    // protected virtual void OnTriggerEnter2D(Collider2D other)
    // {
    //     base.OnTriggerEnter2D(other); 

    //     if (other.CompareTag("Enemy"))
    //     {
    //         // Debug.Log("Trigger con: " + other.name + " (tag: " + other.tag + ")");
    //         if (other.CompareTag("Enemy"))
    //         {
    //             //Debug.Log("colisionó un enemigo");

    //             // Usando DOTween para animar la colicion con el barco
    //             // Centralizamos aquí el feedback visual
    //             AplicarFeedbackVisual();
    //             takeDamage(1); 
    //         }
    //     }
    // }

    //efectos con DOTween
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        // 1. REGLA DE ORO: Si el impacto es detectado por un trigger hijo (marinero)
        // y no por el propio collider del barco, lo ignoramos.

        // Obtenemos el collider que recibió el contacto en el Barco
        // (Esto requiere que el Barco tenga su propio Collider2D)
        
        Collider2D miCollider = GetComponent<Collider2D>();

        // Si el contacto NO fue con el collider principal del barco, salimos.
        // Esto filtra cualquier colisión que venga de los marineros.
        if (!miCollider.IsTouching(other)) return;

        // 2. FILTRO DE TAGS
        if (other.CompareTag("Enemy") || other.CompareTag("Boss") || other.CompareTag("BulletEnemy"))
        {
            // Si el 'other' es un trigger (como una bala que no choca físicamente), 
            // podrías querer recibir daño igual, pero si es el rango del marinero, NO.
            // Por eso la línea 'miCollider.IsTouching(other)' es la más segura.
            shipSound.Danio(); //sonido de daño
            AplicarFeedbackVisualDaño();
            takeDamage(1);
            Debug.Log("Impacto REAL en el casco detectado con: " + other.tag);
        }
        else if (other.CompareTag("Cofre"))
        {
            shipSound.Cofre(); // Reproduce el sonido de recolectar cofre
            AplicarFeedbackRecoleccion();
        }
    }

    // Feedback para cuando recibimos daño (CON TEMBLOR)
    private void AplicarFeedbackVisualDaño()
    {
        var sequence = DOTween.Sequence();
        sequence.Insert(0, spr.DOColor(Color.red, 0.1f));
        sequence.Insert(0, Camera.main.transform.DOShakePosition(0.2f, 0.15f)); // El temblor se queda aquí
        sequence.Insert(0.1f, spr.DOColor(Color.white, 0.1f));
    }

    // Feedback para el cofre (SIN TEMBLOR, más amable)
    private void AplicarFeedbackRecoleccion()
    {
        // En lugar de temblar, podemos hacer que el barco brille un poco
        // o se agrande levemente (punch scale) para dar una sensación positiva
        var sequence = DOTween.Sequence();
        sequence.Insert(0, spr.DOColor(Color.yellow, 0.1f)); // Flash amarillo de oro
        sequence.Insert(0, Ships.DOScale(1.1f, 0.1f).SetLoops(2, LoopType.Yoyo)); // Pequeño salto de escala
        sequence.Insert(0.1f, spr.DOColor(Color.white, 0.1f));
    }

    // Método de apoyo para organizar el "embellecimiento" con DOTween
    private void AplicarFeedbackVisual()
    {
        var sequence = DOTween.Sequence();
        sequence.Insert(0, spr.DOColor(Color.red, 0.1f));
        sequence.Insert(0, Camera.main.transform.DOShakePosition(0.2f, 0.15f));
        sequence.Insert(0.1f, spr.DOColor(Color.white, 0.1f));
    }



    private void ActualizarVisualVida()
    {
        if (barraVidaOndulante != null)
        {
            float porcentaje = (float)HP / hpMaximo;
            barraVidaOndulante.SetHealth(porcentaje);
        }
    }




    // modifico take damage para no morir de ataques constantes del jefe
    public override void takeDamage(int damage)
    {
        if (!isAlive || esInvulnerable) return;

        HP -= damage;

        // NUEVO: Actualizamos la barra de vida ondulante
        ActualizarVisualVida();

        if (HP <= 0)
        {
            isAlive = false;
            SceneManager.LoadScene("prefabs");
        }
        else
        {
            StartCoroutine(RutinaInvulnerabilidad());
        }
    }

    IEnumerator RutinaInvulnerabilidad()
    {
        esInvulnerable = true;
        if (spr != null)
        {
            spr.DOFade(0.5f, 0.2f).SetLoops(-1, LoopType.Yoyo);
        }

        yield return new WaitForSeconds(1.5f);

        if (spr != null)
        {
            spr.DOKill();
            spr.color = Color.white;
        }
        esInvulnerable = false;
    }
}
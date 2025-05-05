using UnityEngine;
using System.Collections;

public class Arrastrable3D : MonoBehaviour
{
    private Camera cam;
    private bool arrastrando = false;
    private float distanciaZ;

    private bool dentroDeLicuadora = false;
    private bool yaProcesado = false;

    public LicuadoraManager licuadoraManager; // ← Referencia pública

    // Posiciones destino
    private Vector3 posIntermedia = new Vector3(-0.14f, 2.23f, 1.06f);
    private Vector3 posFinal = new Vector3(-0.14f, -1.44f, 2.64f);

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform == transform)
                {
                    arrastrando = true;
                    distanciaZ = Vector3.Distance(transform.position, cam.transform.position);
                }
            }
        }

        if (arrastrando && Input.GetMouseButton(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            Vector3 punto = ray.GetPoint(distanciaZ);
            transform.position = punto;
        }

        if (arrastrando && Input.GetMouseButtonUp(0))
        {
            arrastrando = false;

            if (dentroDeLicuadora && !yaProcesado)
            {
                StartCoroutine(ProcesarIngrediente());
            }
        }
    }

    private IEnumerator ProcesarIngrediente()
    {
        if (yaProcesado) yield break;
        yaProcesado = true;

        Vector3 posicionInicio = transform.position;

        // Fase 1: hacia punto intermedio
        yield return StartCoroutine(MoverConParabola(posicionInicio, posIntermedia, 0.5f));

        // Fase 2: hacia punto final
        yield return StartCoroutine(MoverConParabola(posIntermedia, posFinal, 0.5f));

        // Agregar al licuadora manager
        Ingrediente ingrediente = GetComponent<Ingrediente>();
        if (ingrediente != null && licuadoraManager != null)
        {
            licuadoraManager.AgregarIngrediente(ingrediente);
        }

        gameObject.SetActive(false);
    }

    private IEnumerator MoverConParabola(Vector3 inicio, Vector3 fin, float duracion)
    {
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / duracion;
            Vector3 punto = Vector3.Lerp(inicio, fin, t);
            float alturaMaxima = 0.6f;
            punto.y += Mathf.Sin(t * Mathf.PI) * alturaMaxima;
            transform.position = punto;
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Licuadora"))
        {
            dentroDeLicuadora = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Licuadora"))
        {
            dentroDeLicuadora = false;
        }
    }
}

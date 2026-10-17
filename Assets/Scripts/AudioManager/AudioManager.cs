using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [Header("Fuente de audio")]
    public AudioSource fuenteMusica;
    public AudioSource fuenteSFX;

    [Header("Clips de sonido")]
    public AudioClip musicaFondo;
    public AudioClip sonidoSalto;
    public AudioClip sonidoGolpe;
    public AudioClip sonidoMoneda;
    public AudioClip sonidoDanio;
    public AudioClip sonidoDanioEnemigo;


    void Awake()
    {
        if(Instance == null)
        {
            Instance=this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(musicaFondo != null && fuenteMusica != null)
        {
            fuenteMusica.clip = musicaFondo;
            fuenteMusica.loop = true;
            fuenteMusica.Play();
        }
    }

  //  public void ReproducirSfx(AudioClip clip)
   // {
      //  if(clip != null && fuenteSFX != null)
       // {
       //     fuenteSFX.PlayOneShot(clip);
        //}
    //}

    // Update is called once per frame
    public void ReproducirSfx(AudioClip clip)
{
    if (clip != null && fuenteSFX != null)
    {
        Debug.Log(">>> Reproduciendo sonido: " + clip.name);
        fuenteSFX.PlayOneShot(clip);
    }
    else
    {
        Debug.LogWarning(">>> Error: clip o fuenteSFX es nulo.");
    }
}


}

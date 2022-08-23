using UnityEngine;

public class ColourChange : MonoBehaviour
{
    public SpriteRenderer dave;
    
    public Color32 randomColour;
    float timer = 0f;
    public float flashSpeed = 0.25f;
    public Vector2 red = new Vector2(0, 255);
    public Vector2 green = new Vector2(0, 255);
    public Vector2 blue = new Vector2(0, 255);

    // Start is called before the first frame update
    void Start()
    {
        dave = GameObject.FindGameObjectWithTag("Player").GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerHandler.stronk)
        {
            timer += Time.deltaTime;
            if (timer >= flashSpeed)
            {
                randomColour.a = 255;
                randomColour.r = (byte)Random.Range(red.x, red.y);
                randomColour.r = (byte)Random.Range(green.x, green.y);
                randomColour.r = (byte)Random.Range(blue.x, blue.y);
                dave.color = randomColour;
            }
        }
        else
        {
            if (dave.color != Color.white)
            {
                timer = 0;
                dave.color = Color.white;
            }
        }
    }
}

using UnityEngine;

// The CharacterMenu class alows the player to choose a PlayerClass
public class CharacterMenu : MonoBehaviour {
    private PlayerCharacter character;

    // Destroys the current PlayerClass if there is any and creates a new one.
    // This method is called by the buttons on the CharacterMenu panel.    
	public void SetPlayerCharacter(PlayerCharacter character)
    {
        if (this.character != null)
        {
            Destroy(this.character);
        }
        this.character = Instantiate(character);

        gameObject.SetActive(false);
    }
}

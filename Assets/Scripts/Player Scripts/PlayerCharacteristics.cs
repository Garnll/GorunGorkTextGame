using UnityEngine;

/// <summary>
/// Las caracteristicas principales del jugador.
/// </summary>
public class PlayerCharacteristics : MonoBehaviour {

    public float defaultStrength = 1;
	[HideInInspector] public float currentStrength;
    public float defaultIntelligence = 1;
	[HideInInspector] public float currentIntelligence;
    public float defaultResistance = 1;
	[HideInInspector] public float currentResistance;
    public float defaultDexterity = 1;
	[HideInInspector] public float currentDexterity;

	public int defaultPods = 10;
	[HideInInspector] public int usedPods;
	[HideInInspector] public int currentMaxPods;

    /// <summary>
    /// x es infravisión.
    /// y es supravisión.
    /// </summary>
    [HideInInspector] public Vector2 vision;
    public Vector2 defaultVision = new Vector2(-4, 4);
    public Race playerRace;
    public Job playerJob;
    public PlayerOther other;


    public void InitializeCharacteristics()
    {
        ChangeStats();
        vision = defaultVision;
    }

    /// <summary>
    /// Devuelve los valores actuales de los stats a sus defaults;
    /// </summary>
    public void ChangeStats()
    {
        currentDexterity = defaultDexterity;
        currentIntelligence = defaultIntelligence;
        currentResistance = defaultResistance;
        currentStrength = defaultStrength;
		currentMaxPods = defaultPods;
    }

    /// <summary>
    /// Cambia los parametros de la visión. El primer factor aumenta o disminuye la infravisión,
    /// el segundo aumenta o disminuye la supravisión.
    /// </summary>
    /// <param name="InfraFactor"></param>
    /// <param name="SupraFactor"></param>
    public void ChangeVision(int InfraFactor, int SupraFactor)
    {
        vision.x = defaultVision.x - InfraFactor;
        vision.y = defaultVision.y + SupraFactor;
    }

    public bool AddPointsToDefaultStrength(int howMany)
    {
        defaultStrength += howMany;
        return true;
    }

    public bool AddPointsToDefaultDexterity(int howMany)
    {
        defaultDexterity += howMany;
        return true;
    }

    public bool AddPointsToDefaultIntelligence(int howMany)
    {
        defaultIntelligence += howMany;
        return true;
    }

    public bool AddPointsToDefaultResistance(int howMany)
    {
        defaultResistance += howMany;
        return true;
    }

	public bool AddPointsToDefaultPods(int howMany) {
		defaultPods += howMany;
		return true;
	}

	public void modifyStrengthBy(float amount) {
		currentStrength += amount;
	}

	public void modifyIntelligenceBy(float amount) {
		currentIntelligence += amount;
	}

	public void modifyDexterityBy(float amount) {
		currentDexterity += amount;
	}

	public void modifyResistanceBy(float amount) {
		currentResistance += amount;
	}

	public void modifyPodsBy(int amount) {
		currentMaxPods += amount;
	}

	public void modifyInfravisionBy(int amount) {
		vision.x -= amount;
	}

	public void modifySupravisionBy(int amount) {
		vision.y += amount;
	}

	public bool checkPodsWith(int weight) {
		if (usedPods + weight > currentMaxPods) {
			return false;
		}
		return true;
	}

	public void addWeight(int weight) {
		usedPods += weight;
	}

	public void removeWeight(int weight) {
		usedPods -= weight;
	}


}

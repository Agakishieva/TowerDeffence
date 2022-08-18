using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CartoonFX
{
	public class CFXR_CountdawnText : MonoBehaviour
	{
		public ParticleSystem partSystem;
		public CFXR_ParticleText_Runtime runtimeParticleText;

		public int startCountFrom;

		void OnEnable()
		{
			InvokeRepeating("SetRandomText", 0f, 1.1f);
		}

		void OnDisable()
		{
			CancelInvoke("SetRandomText");
			partSystem.Clear(true);
		}

		void SetRandomText()
		{
			print($"COUNT {startCountFrom}");
			// destroy
			if (startCountFrom == 0)
            {
				Destroy(gameObject);
            }
			// set text size according to the damage amount
			int damage = Random.Range(10, 1000);
			runtimeParticleText.size = Mathf.Lerp(0.8f, 1.3f, damage / 1000f);

			// update text
			string text = startCountFrom.ToString(); // damage.ToString();
			runtimeParticleText.GenerateText(text);

			startCountFrom -= 1;
			partSystem.Play(true);
		}
	}
}

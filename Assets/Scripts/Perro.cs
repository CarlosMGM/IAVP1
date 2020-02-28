using UnityEngine;

namespace UCM.IAV.Movimiento
{
	public class Perro : Animal
	{

        private Huida huida;

        new private void Start()
        {
            base.Start();
            huida = GetComponent<Huida>();
        }

        // Update is called once per frame
        public new void Update()
		{
			base.Update();

			if (!sound)
			{
				seguir.SeguirJugador();
			}
			else
			{
                huida.HuirDeJugador();
			}
		}
	}
}
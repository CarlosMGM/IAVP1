using UnityEngine;

namespace UCM.IAV.Movimiento
{
	public class Perro : Animal
	{
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
				Run();
				var dir = new Direccion {lineal = new Vector3(-1.0f, 0.0f, -1.0f)};
				if (mezclarPorPeso)
					SetDireccion(dir, seguir.peso);
				else if (mezclarPorPrioridad)
					SetDireccion(dir, seguir.prioridad);
				else
					SetDireccion(dir);
			}
		}
	}
}
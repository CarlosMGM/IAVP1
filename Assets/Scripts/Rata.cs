using UnityEngine;

namespace UCM.IAV.Movimiento
{
    [RequireComponent(typeof(RandomMovement))]
    public class Rata : Animal
	{
        RandomMovement randomMovement;

        new private void Start()
        {
            base.Start();
            randomMovement = GetComponent<RandomMovement>();
            randomMovement.transformObjetivo = transform.parent.GetChild(0);
        }

        // Update is called once per frame
        public new void Update()
		{
			base.Update();

			if (sound)
			{
				seguir.SeguirJugador();
			}
			else
			{
				Run();
                var dir = randomMovement.GetDireccion();
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
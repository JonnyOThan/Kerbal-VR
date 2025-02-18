﻿using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KerbalVR
{
	[KSPAddon(KSPAddon.Startup.Flight, false)]
	internal class KerbalVR_FreeIVA : MonoBehaviour
	{
		void Awake()
		{
			if (!Core.IsVrEnabled)
			{
				Component.Destroy(this);
				return;
			}

			FreeIva.KerbalIvaAddon.GetInput += FreeIva_GetInput;
		}

		void OnDestroy()
		{
			if (!Core.IsVrEnabled) return;

			FreeIva.KerbalIvaAddon.GetInput -= FreeIva_GetInput;
		}

		void KeepMax(ref float a, ref float b)
		{
			if (Mathf.Abs(a) > Mathf.Abs(b))
			{
				b = 0;
			}
			else
			{
				a = 0;
			}
		}

		private void FreeIva_GetInput(ref FreeIva.KerbalIvaAddon.IVAInput input)
		{
			FirstPersonKerbalFlight.Instance.GetKerbalRotationInput(out float yaw, out float pitch, out float roll);

			if (FreeIva.KerbalIvaAddon.Instance.KerbalIva.UseRelativeMovement())
			{
				pitch = 0.0f;
				FreeIva.KerbalIvaAddon.Instance.KerbalIva.currentRelativeOrientation.x = 0;
			}

			// restrict rotations to a single axis
			KeepMax(ref yaw, ref pitch);
			KeepMax(ref pitch, ref roll);
			KeepMax(ref yaw, ref roll);

			yaw *= yawRate;
			pitch *= pitchRate;
			roll *= rollRate;

			if (input.MovementThrottle == Vector3.zero)
			{
				input.MovementThrottle = FirstPersonKerbalFlight.Instance.GetKerbalMovementThrottle();

				if (FreeIva.KerbalIvaAddon.Instance.KerbalIva.UseRelativeMovement())
				{
					float verticalMovement = input.MovementThrottle.y;
					input.MovementThrottle.y = 0;

					input.MovementThrottle = InternalCamera.Instance.transform.localRotation * input.MovementThrottle;
					input.MovementThrottle.y = verticalMovement;
				}
				else
				{
					input.MovementThrottle = InternalCamera.Instance.transform.localRotation * input.MovementThrottle;
				}
			}
			if (input.RotationInputEuler == Vector3.zero)
			{
				input.RotationInputEuler = new Vector3(pitch, yaw, roll);

				if (!FreeIva.KerbalIvaAddon.Instance.KerbalIva.UseRelativeMovement())
				{
					Quaternion inputRotation = InternalCamera.Instance.transform.localRotation * Quaternion.Euler(input.RotationInputEuler) * Quaternion.Inverse(InternalCamera.Instance.transform.localRotation);
					input.RotationInputEuler = inputRotation.eulerAngles;

					if (input.RotationInputEuler.x > 180) input.RotationInputEuler.x -= 360;
					if (input.RotationInputEuler.y > 180) input.RotationInputEuler.y -= 360;
					if (input.RotationInputEuler.z > 180) input.RotationInputEuler.z -= 360;
				}
			}

			input.Jump = FirstPersonKerbalFlight.Instance.GetJumpState();
			input.ToggleCrouch = FirstPersonKerbalFlight.Instance.GetCrouchState();
		}

		static float pitchRate = 0.5f;
		static float yawRate = 0.5f;
		static float rollRate = 0.5f;
	}

	[HarmonyPatch(typeof(FreeIva.KerbalIvaAddon), nameof(FreeIva.KerbalIvaAddon.Unbuckle))]
	class KerbalIvaAddon_Unbuckle_Patch
	{
		static void Prefix()
		{
			InteractionSystem.Instance.transform.SetParent(null, false);

			// FreeIVA is going to take the internal camera's rotation as the transform of the kerbal's body,
			// and then try to set the internal camera's local rotation to identity
			// Then the VR system is going to set the internal camera's local rotation back to track the headset.
			// To counteract this, make sure that FreeIVA thinks the internal camera's localrotation is identity
			InternalCamera.Instance.transform.localRotation = Quaternion.identity;
		}

		static void Postfix(FreeIva.KerbalIvaAddon __instance)
		{
			// note: this creates a VRAnchor for the internal camera so that wobble etc can be applied
			FirstPersonKerbalFlight.Instance.FixInternalCamera();
			KerbalVR.Core.SetActionSetActive("EVA", true);
			// zero out the anchor's local transform so that we're directly connected to the kerbal body
			InternalCamera.Instance.transform.parent.localPosition = Vector3.zero;
			InternalCamera.Instance.transform.parent.localRotation = Quaternion.identity;
		}
	}

	[HarmonyPatch(typeof(FreeIva.KerbalIvaAddon), nameof(FreeIva.KerbalIvaAddon.Buckle))]
	class KerbalIvaAddon_Buckle_Patch
	{
		static void Postfix(FreeIva.KerbalIvaAddon __instance)
		{
			// buckle is going to need a lot more work, but at a minmum we need to restore the vr anchor
			FirstPersonKerbalFlight.Instance.FixIVACamera();
			KerbalVR.Core.SetActionSetActive("EVA", false);
		}
	}
}

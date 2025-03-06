# <marquee behavior="alternate" scrollamount="10" style="font-size:3em; color:#FF4500;">Ascenso del Caos</marquee>

## Descripción

**Ascenso del Caos** es un juego desarrollado en Unity que combina acción, estrategia y elementos de disparos en un entorno dinámico. Este proyecto está pensado para ser usado como base, ya sea para aprender desarrollo de videojuegos o para crear tu propio juego a partir de esta estructura.

## Características

- **Sistema de Disparo:**  
  Implementa la física de proyectiles y colisiones mediante el script `Bullet.cs`.

- **Generación de Objetos y Enemigos:**  
  Utiliza el script `spawn.cs` para instanciar enemigos y otros objetos en áreas definidas, evitando superposiciones.

- **Control del Jugador:**  
  Maneja el movimiento, animaciones y disparos con `PlayerController.cs` y `PlayerWeapon.cs`.

- **Interfaz de Usuario:**  
  Cuenta con elementos UI para mostrar la salud (`HealthUI.cs`), la puntuación (`PuntuacionUI.cs`) y menús para finalizar el juego (`FinMenu.cs`).

- **Seguimiento de Cámara:**  
  El script `CameraFollow.cs` permite que la cámara siga al jugador de manera suave.

- **Sistema de Salud de Enemigos:**  
  Mediante `EnemyHealth.cs`, cada enemigo muestra visualmente su barra de vida.

## Uso del Proyecto

Este repositorio se puede utilizar **como base** para desarrollar tus propios juegos o para aprender sobre el desarrollo de videojuegos con Unity.

### Requisitos

- **Unity:** Versión compatible con el proyecto.
- **Conocimientos básicos de C# y Unity.**

### Instalación

1. **Clona el repositorio:**
   ```bash
   git clone https://github.com/tu_usuario/ascenso-del-caos.git

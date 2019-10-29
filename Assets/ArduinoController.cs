using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;

public class ArduinoController : MonoBehaviour
{
    private SerialPort arduino;

    public string portName = "COM2";

    public int baudRate = 9600;

    private string leituraAnterior = "0";

    public MeshRenderer cubo1;
    public MeshRenderer cubo2;
    public MeshRenderer cubo3;

    private bool keyPressAnterior = false;

    private void Start()
    {
        try
        {
            arduino = new SerialPort(portName, baudRate)
            {
                ReadTimeout = 200
            };

            arduino.Open();
        }
        catch (System.Exception)
        {
        }
    }

    private void Update()
    {
        //var leitura = arduino.ReadLine();

        var leitura = SimulateReadLine();

        var keyDown = leitura == "1" && leituraAnterior == "0";
        var keyPress = leitura == "1";
        var keyUp = leitura == "0" && leituraAnterior == "1";

        // Processa keyDown/Press/Up, caso necessário

        if (keyDown)
        {
            AlterarCorCubo(cubo1);
            StartCoroutine(ApagarCubo(cubo1));
        }

        if (keyPress)
        {
            AlterarCorCubo(cubo2);
        }
        else if (keyPressAnterior)
        {
            StartCoroutine(ApagarCubo(cubo2));
        }

        keyPressAnterior = keyPress;

        if (keyUp)
        {
            AlterarCorCubo(cubo3);
            StartCoroutine(ApagarCubo(cubo3));
        }

        leituraAnterior = leitura;
    }

    private void AlterarCorCubo(MeshRenderer cubo)
    {
        cubo.material.color = Color.red;
    }

    private IEnumerator ApagarCubo(MeshRenderer cubo)
    {
        yield return new WaitForSeconds(0.2f);

        cubo.material.color = Color.white;
    }

    private string SimulateReadLine()
    {
        return Input.GetKey(KeyCode.Z) ? "1" : "0";
    }
}
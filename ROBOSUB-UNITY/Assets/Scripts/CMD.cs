using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System.Threading.Tasks;


public class CMD : MonoBehaviour
{
    public bool go_again = true;
    public Process cmd = new Process();
    private string cmdLine = "";
    private string cmdOutput = "";

    // Start is called before the first frame update
    void Start()
    {
        cmd.StartInfo.FileName = "CMD.exe";
        cmd.StartInfo.UseShellExecute = false;
        cmd.StartInfo.RedirectStandardInput = true;
        cmd.StartInfo.RedirectStandardOutput = true;
        cmd.StartInfo.RedirectStandardError = true;
        cmd.StartInfo.Arguments = "/k echo Hello World";
        cmd.StartInfo.CreateNoWindow = true;
        cmd.Start();

    }

    // Update is called once per frame
    void Update()
    {
        
        if (go_again){
        go_again = false;
        StartCoroutine(Read_console());
        //Read_console2();
        TestCMD();
        
        }
    }

    public void TestCMD(){
        

        writeConsole("cd ..");
        
        writeConsole("cd developmentEnvironment");
        
        writeConsole("python make.py build");
        
        
        writeConsole("docker exec developmentenvironment-sil-1 ./development/robotCode/competitionCode2024/ros2_ws/bash_scripts/start_for_unity.sh");
        
        
        

    }

    public void writeConsole(string command){
        cmd.StandardInput.Write(command);
        //cmd.StandardInput.Flush();
        
    }

    public IEnumerator Read_console(){
        cmdLine = "full";
        cmdOutput = "";
        while(cmdOutput != "STOP"){
            yield return new WaitForSeconds(0.1f);
            cmdLine = cmd.StandardOutput.ReadLine();
            
            if(cmdLine != ""){
                UnityEngine.Debug.Log(cmdLine);
                
            }
            else{
                cmdOutput = "STOP";
            }
        }
        //UnityEngine.Debug.Log(cmdOutput);
    }


}

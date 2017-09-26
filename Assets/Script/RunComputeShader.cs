using UnityEngine;
using System.Collections;

public class RunComputeShader : MonoBehaviour 
{

    public ComputeShader shader;

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    

    /// <summary>
    /// There are a few things to note here. First is setting the enableRandomWrite flag of your render texture BEFORE you create it. 
    /// This gives your compute shaders access to write to the texture. 
    /// If you don’t set this flag you won’t be able to use the texture as a write target for the shader.

    //Next we need a way to identify what function we want to call in our compute shader. 
    //The FindKernel function takes a string name, which corresponds to one of the kernel names we set up at the beginning of our compute shader. 
    //Remember, a Compute Shader can have multiple kernels (functions) in a single file.

    //The ComputeShader.SetTexture call lets us move the data we want to work with from CPU memory to GPU memory. 
    //Moving data between memory spaces is what will introduce latency to your program, 
    //and the amount of slowdown you see is proportional to the amount of data that you are transferring. 
    //For this reason, if you plan on running a compute shader every frame you’ll need to aggressively optimize how much data is actually get operated on.

    //The three integers passed to the Dispatch call specify the number of thread groups we want to spawn. 
    //Recall that each thread group’s size is specified in the numthreads block of the compute shader, so in the above example, 
    //the number of total threads we’re spawning is as follows:

    //32*32 thread groups * 64 threads per group = 65536 threads total.

    //This ends up equating to 1 thread per pixel in the render texture, 
    //which makes sense given that the kernel function can only operate on 1 pixel per call.

    /// </summary>
 

    void RunShader()
    {
        VecMatPair[] data = new VecMatPair[5];
        VecMatPair[] output = new VecMatPair[5];

        //INITIALIZE DATA HERE

        ComputeBuffer buffer = new ComputeBuffer(data.Length, 76);
        int kernel = shader.FindKernel("Multiply");
        shader.SetBuffer(kernel, "dataBuffer", buffer);
        shader.Dispatch(kernel, data.Length, 1, 1);
        buffer.GetData(output);
    }

    struct VecMatPair
    {
        public Vector3 point;
        public Matrix4x4 matrix;
    }
}

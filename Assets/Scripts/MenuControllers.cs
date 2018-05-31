using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI; // Required when Using UI elements.

public class MenuControllers : MonoBehaviour {

    public InputField pathToFile;
    public InputField nSize;

    public Material redMat;
    public Material greenMat;
    public Material blueMat;
    public Material whiteMat;
    public Material blackMat;

    public GameObject baseCube;
    public GameObject MapContainer;

    private FileInfo fileInfo;
    private int offSetInt;
    private int bitmapSizeInt;
    private int vDim;
    private int hDim;
    private Color[,] colorsMatrix;
    private char[,] colorsMatrixNormalized;
    private string colorsMatrisString;
    private byte[] data;

    // Exit program.
    public void ExitGame()
    {
        Application.Quit();
    }

    // Loads the image from the path.
    public void LoadImageFromPath()
    {
        byte[] offSet = { 0, 0, 0, 0 };
        byte[] bitmapSize = { 0, 0, 0, 0 };
        byte[] vDimB = { 0, 0, 0, 0 };
        byte[] hDimB = { 0, 0, 0, 0 };

        try
        {
            // Load file meta data with FileInfo
            fileInfo = new FileInfo(pathToFile.text);
            
            // The byte[] to save the data in
            data = new byte[fileInfo.Length];

            // Load a filestream and put its content into the byte[]
            using (FileStream fs = fileInfo.OpenRead())
            {
                fs.Read(data, 0, data.Length);
            }

            // Process data
            // Get Bitmap offSet to Pixel Array.
            for (var i = 0; i < 4; i++)
            {
                offSet[i] = data[i + 10];
            }
            offSetInt = BitConverter.ToInt32(offSet, 0);
            // Debug.Log("Off set value is: " + offSetInt);

            // Get size of the raw bitmap data (including padding).
            for (var i = 0; i < 4; i++)
            {
                bitmapSize[i] = data[i + 34];
            }
            bitmapSizeInt = BitConverter.ToInt32(bitmapSize, 0);
            // Debug.Log("Raw bitmap size is: " + bitmapSizeInt);

            // Get Vertical Dimension of bitmap image.
            for (var i = 3; i >= 0; i--)
            {
                vDimB[i] = data[i + 18];
            }
            vDim = BitConverter.ToInt32(vDimB, 0);
            // Debug.Log("Vertical dim is: " + vDim);

            // Get Horizontal Dimension of bitmap image.
            for (var i = 3; i >= 0; i--)
            {
                hDimB[i] = data[i + 22];
            }
            hDim = BitConverter.ToInt32(hDimB, 0);
            // Debug.Log("Horizontal dim is: " + hDim);

            // Now I normalize the image.
            if ( (nSize.text != "") && (Int32.Parse(nSize.text) > 0) )
            {
                NormalizeImage(data, Int32.Parse(nSize.text));
            }
            else
            {
                pathToFile.text = "Tamaño 'n' incorrecto.";
            }

        } catch
        {
            pathToFile.text = "File not found.";
        }
    }

    // Functions to know is the color
    /*
        * 
        * For this program we will consider the following
        * - Red   is when first value is higer than the other by 40.
        * - Green is when second value is higer than the other by 40.
        * - Blue  is when third value is higer than the other by 40.
        * - White is when all values are over 116.
        * - Black is anything else.
        * 
    */
    private Boolean isRed(float r, float g, float b)
    {
        if (r > g && r > b)
        {
            // Calculate diferences
            float rg = r - g;
            float rb = r - b;
            if (rg >= 40f && rb >= 40f)
            {
                // Then it's red
                return true;
            }
        }
        // If any of the If's fail, then it's not red.
        return false;
    }

    private Boolean isGreen(float r, float g, float b)
    {
        if (g > r && g > b)
        {
            // Calculate diferences
            float gr = g - r;
            float gb = g - b;
            if (gr >= 40f && gr >= 40f)
            {
                // Then it's green
                return true;
            }
        }
        // If any of the If's fail, then it's not green.
        return false;
    }

    private Boolean isBlue(float r, float g, float b)
    {
        if (b > r && b > g)
        {
            // Calculate diferences
            float br = b - r;
            float bg = b - g;
            if (br >= 40f && bg >= 40f)
            {
                // Then it's blue
                return true;
            }
        }
        // If any of the If's fail, then it's not blue.
        return false;
    }

    private Boolean isWhite(float r, float g, float b)
    {
        float whiteValue = 116f;

        if (r >= whiteValue && g >= whiteValue && b >= whiteValue)
        {
            // If all values are over 116 then it's white.
            return true;
        }

        // Else it's black.
        return false;
    }

    // Normalize image.
    private void NormalizeImage(byte[] data, int n)
    {
        // Initialize colors matrix.
        colorsMatrixNormalized = new Char[n, n];
        colorsMatrix = new Color[vDim, hDim];

        // Counter for xPixels on each row.
        int xPixels = vDim - 1;
        int yPixels = 0;

        // I know my pixel array start at:
        // ((vDim * 3)) * hDim 
        // And ends at:
        // offSetint
        // Make a list array into a 3D Matrix of colors
        for ( var xy = ((vDim * hDim) * 3) - 4; xy > -1; xy -= 3)
        {
            // Check for the matrix limit.
            if (xPixels == -1)
            {
                xPixels = vDim - 1;
                yPixels++;
            }
            // With this I can know what color (Blue, Green, Red, Alpha) I have.
            Color pixelColor = new Color(data[xy + 3], data[xy + 2], data[xy + 1]);

            // Now add the color to the matrix on the right position.
            colorsMatrix[xPixels, yPixels] = pixelColor;

            xPixels--;
        }

        // Size of chunk
        int xChunkSize = (int)Mathf.Floor (vDim / n);
        int yChunkSize = (int)Mathf.Floor(hDim / n);

        // Color variables to average
        float red = 0;
        float green = 0;
        float blue = 0;

        // A flag to know that I have not found a green spot.
        bool greenSpot = false;
        // A flag to know that I have not found a red spot.
        bool redSpot = false;

        for (var y = 0; y < n; y++)
        {
            for (var x = 0; x < n; x++)
            {
                // Reset rbg variables
                red = green = blue = 0f;

                // Here I have to get chunks of data
                for (var xx = (xChunkSize * x); xx < (xChunkSize * (x + 1)); xx++)
                {
                    for (var yy = (yChunkSize * y); yy < (yChunkSize * (y + 1)); yy++)
                    {
                        // Now I have to average the colors to know what color is the [x, y] cell.
                        red += colorsMatrix[xx, yy].r;
                        green += colorsMatrix[xx, yy].g;
                        blue += colorsMatrix[xx, yy].b;
                    }
                }

                // After having the sum of all the colors
                // We build a color from the average of those.
                red = red / (xChunkSize * yChunkSize * 1.0f);
                green = green / (xChunkSize * yChunkSize * 1.0f);
                blue = blue / (xChunkSize * yChunkSize * 1.0f);

                // Here we select what color is this.
                if (isRed(red, green, blue))
                {
                    if (redSpot)
                    {
                        // Assign white to [x, y] normalizedColorsMatrix
                        colorsMatrixNormalized[x, y] = 'w';
                    }
                    else {
                        // Check for red flag.
                        redSpot = true;
                        // Assign red to [x, y] normalizedColorsMatrix
                        colorsMatrixNormalized[x, y] = 'r';
                    }
                        
                }
                else if (isGreen(red, green, blue))
                {
                    // Check for green flag.
                    greenSpot = true;
                    // Assign green to [x, y] normalizedColorsMatrix
                    colorsMatrixNormalized[x, y] = 'g';
                }
                else if (isBlue(red, green, blue))
                {
                    // Assign white to [x, y] normalizedColorsMatrix
                    colorsMatrixNormalized[x, y] = 'w';
                }
                else if (isWhite(red, green, blue))
                {
                    // Assign white to [x, y] normalizedColorsMatrix
                    colorsMatrixNormalized[x, y] = 'w';
                }
                else
                {
                    // Assign black to [x, y] normalizedColorsMatrix
                    colorsMatrixNormalized[x, y] = 'b';
                }

            }
        }
    }

    public void makeMap()
    {
        // Clear the map holder.
        foreach (Transform child in MapContainer.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        // initialize n
        int n = Int32.Parse(nSize.text);
        colorsMatrisString = "";

        Vector3 myPos;
        GameObject tempCube;

        for (var y = 0; y < n; y++)
        {

            for (var x = 0; x < n; x++)
            {
                // Set the position and initialize the clone GameObject
                myPos = new Vector3(x, 0.0f, -1.0f * y);
                tempCube = Instantiate(baseCube, myPos, Quaternion.identity) as GameObject;

                // Choose what color to paint the cube.
                switch (colorsMatrixNormalized[x, y])
                {
                    case 'r':
                        tempCube.GetComponent<Renderer>().material = redMat;
                        break;
                    case 'g':
                        tempCube.GetComponent<Renderer>().material = greenMat;
                        break;
                    case 'w':
                        tempCube.GetComponent<Renderer>().material = whiteMat;
                        break;
                    default:
                        tempCube.GetComponent<Renderer>().material = blackMat;
                        break;
                }
                // Create a string form of the matrix.
                colorsMatrisString += colorsMatrixNormalized[x, y] + ",";

                // Add the clone cube to the map container GameObject.
                tempCube.transform.parent = MapContainer.transform;
            }
            // Remove the last ','
            colorsMatrisString = colorsMatrisString.Substring(0, colorsMatrisString.Length - 1);
            // Add a row divider. 
            colorsMatrisString += "|";
        }
        // Remove the las '|'.
        colorsMatrisString = colorsMatrisString.Substring(0, colorsMatrisString.Length - 1);
    }

    public void solveLab(string algorithm)
    {
        FrameWork myProblem = new FrameWork(colorsMatrixNormalized, Int32.Parse(nSize.text));
        foreach (var element in myProblem.initialStates)
        {
            Debug.Log(element[0] + ", " + element[1] + " 2");
        }
        GraphSearch graph = new GraphSearch(myProblem, 1);
        List<int[]> path = graph.JustDoIt();
        foreach (var element in path)
        {
            Debug.Log(element[0] + ", " + element[1]);
        }
    }

    
}

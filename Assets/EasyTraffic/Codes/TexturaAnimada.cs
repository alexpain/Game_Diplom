using UnityEngine;
using System.Collections;

public class TexturaAnimada : MonoBehaviour 
	{
	
	public	int			QTD_Linhas;
	public	int 		QTD_Colunas;
	
			int			Linha_Atual;
			int			Coluna_Atual;
	
			Vector2		Atualizador;
	
	public	float		Frequencia;
	
			float		Tempo;
	
	public	bool		VersoReverso;
			bool		Estado;
	
			float		TimeDeath;
	
	void Muda_Posicao()
		{
		Vector2 at 	= new Vector2(0,0);
		at.x 		= (Linha_Atual - 1) 			* Atualizador.x;
		at.y 		= (QTD_Colunas - Coluna_Atual) 	* Atualizador.y;
		
		gameObject.GetComponent<Renderer>().material.mainTextureOffset = at;
		}

	// Use this for initialization
	void Start () 
		{
		Estado 				= false;
		
		if(QTD_Linhas == 0) 	{ QTD_Linhas 	= 1; }
		
		if(QTD_Colunas == 0) 	{ QTD_Colunas 	= 1; }
		
		Linha_Atual 	= 1;
		
		Coluna_Atual	= 1;
		
		Atualizador	= 	gameObject.GetComponent<Renderer>().material.mainTextureScale;
		
		Atualizador.x		=	1.0f / QTD_Linhas;
		Atualizador.y		=	1.0f / QTD_Colunas;
		
		gameObject.GetComponent<Renderer>().material.mainTextureScale	= Atualizador;
		
		if(Frequencia == 0.0f)	{ Frequencia = 1.0f; }
		Tempo				= 1.0f;
		
		Muda_Posicao();
		
		TimeDeath			= 0.5f;
		}
	
	void Normal()
		{
		Tempo -= Time.deltaTime * Frequencia; 
			
		if(Tempo <= 0.0f)
			{
			Linha_Atual++;
			if( Linha_Atual > QTD_Linhas )
				{
				Linha_Atual = 1;
				
				Coluna_Atual++;
				
				if(Coluna_Atual > QTD_Colunas) { Coluna_Atual = 1; }
				}
			
			Muda_Posicao();
			
			Tempo = 1.0f;
			}
		}
	
	void VR_Mode()
		{
		Tempo -= Time.deltaTime * Frequencia;
		
		if(Tempo <= 0.0f)
			{
			if(!Estado) 
				{ 
				Linha_Atual++; 
				
				if( Linha_Atual > QTD_Linhas )
					{
					Linha_Atual = 1;
					
					Coluna_Atual++;
					
					if(Coluna_Atual > QTD_Colunas) 
						{ 
						Coluna_Atual = QTD_Colunas;
						Estado = true;
						}
					}
				}
			else 		
				{ 
				Linha_Atual--; 
				
				if( Linha_Atual <= 0 )
					{
					Linha_Atual = QTD_Linhas;
					
					Coluna_Atual--;
					
					if(Coluna_Atual <= 0) 
						{ 
						Coluna_Atual = 1;
						Estado = false;
						}
					}
				}
			
			
			
			Muda_Posicao();
			
			Tempo = 1.0f;
			}
		}
	
	// Update is called once per frame
	void Update () 
		{
		if(!VersoReverso) 	{ Normal(); }
		else  				{ VR_Mode(); }
		
		Camera cameraToLookAt = Camera.main;
		
		if(cameraToLookAt.enabled)
			{
			transform.LookAt(cameraToLookAt.transform.position ); 
			transform.Rotate(-90,0,0);
		
			TimeDeath -= Time.deltaTime;
		
			if(TimeDeath <= 0.0f)
				{
				Destroy(this.gameObject);
				}
			}
		}
	
	}

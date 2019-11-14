# ExtendCSharp
Questa libreria è pensata per estendere le funzionalità di C# implementanto:
- Estensioni ( Extension.cs )
	Le estensioni sono raggruppate in region in base alla classe di estensione
- Servizi ( Servies )
	I servizi vanno a raggruppare servizi già esistenti o a crearne/organizzarne di nuovi 
	Guardare README_Services per capirne il funzionamento
- Classi Estese ( ExtendedClass)
	Le classi base vengono estense per l'aggiunta di numerose funzioni
- Controlli ( Controls )
	Controlli base ereditati o nuovi gruppi di controlli
- Log ( Log )
	degli strumenti di log per avere una finestra con tutti i dettagli
- ALTRO
	Sono introdotti Attributi, Eccezioni, Form, Interfacce, Strutture e Wrapper per il 
	funzionamento della libreria e dei suoi componenti

Questa libreria inoltre si appoggia alle librerie:
- CsQuery
- CefLibrary
- MySql.Data
- Newtonsoft.JSON


TUTTI I README sono nella cartella apposita: README


Per le integrazioni specifiche in WPF guardare: [ExtendCSharpWPF](https://github.com/Rarder44/ExtendCSharpWPF)

------------------------------------------------------------------------------------------------------------------


Tutorial per includere le DLL come risorsa in automatico:
http://www.paulrohde.com/merging-a-wpf-application-into-a-single-exe/

Questo metodo permette in fase di compilazione di includere come risorsa le DLL esportate nella cartella di output di compilazione; e di risolvere a runtime se non vengono trovate

In breve:
- apro il csproj del progetto eseguibile
- trovo la linea "<Import Project="$(MSBuildToolsPath)\Microsoft.CSharp.targets" />"
- alla lina successiva incollo:
<Target Name="AfterResolveReferences">
  <ItemGroup>
    <EmbeddedResource Include="@(ReferenceCopyLocalPaths)" Condition="'%(ReferenceCopyLocalPaths.Extension)' == '.dll'">
      <LogicalName>%(ReferenceCopyLocalPaths.DestinationSubDirectory)%(ReferenceCopyLocalPaths.Filename)%(ReferenceCopyLocalPaths.Extension)</LogicalName>
    </EmbeddedResource>
  </ItemGroup>
</Target>

- Nel main, come prima istruzione, aggiungo: AppDomain.CurrentDomain.AssemblyResolve += OnResolveAssembly;
- aggiungo nel Program.cs questa funzione:
		// This function is not called if the Assembly is already previously loaded into memory.
        // This function is not called if the Assembly is already in the same folder as the app.
        //
        private static Assembly OnResolveAssembly(object sender, ResolveEventArgs e)
        {
            var thisAssembly = Assembly.GetExecutingAssembly();

            // Get the Name of the AssemblyFile
            var assemblyName = new AssemblyName(e.Name);
            var dllName = assemblyName.Name + ".dll";

            // Load from Embedded Resources
            var resources = thisAssembly.GetManifestResourceNames().Where(s => s.EndsWith(dllName));
            if (resources.Any())
            {
                // 99% of cases will only have one matching item, but if you don't,
                // you will have to change the logic to handle those cases.
                var resourceName = resources.First();
                using (var stream = thisAssembly.GetManifestResourceStream(resourceName))
                {
                    if (stream == null) return null;
                    var block = new byte[stream.Length];

                    // Safely try to load the assembly.
                    try
                    {
                        stream.Read(block, 0, block.Length);
                        return Assembly.Load(block);
                    }
                    catch (IOException)
                    {
                        return null;
                    }
                    catch (BadImageFormatException)
                    {
                        return null;
                    }
                }
            }

            // in the case the resource doesn't exist, return null.
            return null;
        }

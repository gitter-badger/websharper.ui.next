namespace WebSharper.UI.Next.Tests

open WebSharper
open WebSharper.JavaScript

open WebSharper.UI.Next
open WebSharper.UI.Next.Notation
open WebSharper.UI.Next.Templating

[<JavaScript>]
module Client =    
    let [<Literal>] TemplateHtmlPath = __SOURCE_DIRECTORY__ + "/template.html"

    type MyTemplate = Template<TemplateHtmlPath> 

    type Item = { name: string; description: string }

    let RefTest() = 
        let num = ref 0
        [1; 2; 3] |> Seq.iter (fun x -> num := !num + x)
        if !num <> 6 then
            failwith "ref operators failing"

    let Main =
        let myItems =
          ListModel.FromSeq [
            { name = "Item1"; description = "Description of Item1" }
            { name = "Item2"; description = "Description of Item2" }
          ]
 
        let title = View.Const "Starting title"
        let var = Var.Create ""
        let btnVar = Var.Create ()
 
        let doc =
            MyTemplate.Doc(
                Title = title,
                ListContainer =
                    (ListModel.View myItems |> Doc.Convert (fun item ->
                        MyTemplate.ListItem.Doc(
                            Name = View.Const item.name,
                            Description = View.Const item.description,
                            FontStyle = View.Const "normal",
                            FontWeight = View.Const "bold")
                    )),
                MyInput = var,
                MyInputView = View.SnapshotOn "" btnVar.View var.View,
                MyCallback = (fun e -> btnVar := ())
            )

        doc |> Doc.RunById "main"

        RefTest()
        
        Console.Log("Running JavaScript Entry Point..")

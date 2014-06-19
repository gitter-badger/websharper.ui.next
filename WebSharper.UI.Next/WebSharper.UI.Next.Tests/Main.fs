namespace WebSharper.UI.Next.Tests

open IntelliFactory.Html
open IntelliFactory.WebSharper
open IntelliFactory.WebSharper.Sitelets

open WebSharper.UI.Next.Tests.CommentBox
open WebSharper.UI.Next.Tests.PhoneExample
open WebSharper.UI.Next.Tests.TextBenchmark

type Action =
    | Phone
    | Comment
    | CheckBox
    | TextBenchmark
    | TodoList
    | SimpleText
    | MouseChase
    | BarChart

module Controls =

    [<Sealed>]
    type EntryPointComment() =
        inherit Web.Control()

        [<JavaScript>]
        override __.Body =
            CommentBox.CommentBoxExample.main () :> _

    [<Sealed>]
    type EntryPointPhone() =
        inherit Web.Control()

        [<JavaScript>]
        override __.Body =
            PhoneExample.PhoneExample.main () :> _

    [<Sealed>]
    type EntryPointCB() =
        inherit Web.Control()

        [<JavaScript>]
        override __.Body =
            CheckBoxTest.CheckBoxTest.main () :> _

    [<Sealed>]
    type EntryPointTB() =
        inherit Web.Control()

        [<JavaScript>]
        override __.Body =
            TextBenchmark.TextBenchmark.main () :> _

    [<Sealed>]
    type EntryPointTodo() =
        inherit Web.Control()

        [<JavaScript>]
        override __.Body =
            TodoList.TodoList.main () :> _

    [<Sealed>]
    type EntryPointSimpleText() =
        inherit Web.Control()

        [<JavaScript>]
        override __.Body =
            SimpleTextBox.SimpleTextBox.Main () :> _

    [<Sealed>]
    type EntryPointMouseChase() =
        inherit Web.Control()

        [<JavaScript>]
        override __.Body =
            MouseChase.MouseChase.Main () :> _

    [<Sealed>]
    type EntryPointBarChart() =
        inherit Web.Control()

        [<JavaScript>]
        override __.Body =
            BarChart.BarChart.Main () :> _

module Skin =
    open System.Web

    type Page =
        {
            Title : string
            Body : list<Content.HtmlElement>
        }

    let MainTemplate =
        Content.Template<Page>("~/Main.html")
            .With("title", fun x -> x.Title)
            .With("body", fun x -> x.Body)

    let WithTemplate title body : Content<Action> =
        Content.WithTemplate MainTemplate <| fun context ->
            {
                Title = title
                Body = body context
            }

module Site =

    let ( => ) text url =
        A [HRef url] -< [Text text]

    let Links (ctx: Context<Action>) =
        UL [
            LI ["Phone" => ctx.Link Phone]
            LI ["CommentBox" => ctx.Link Comment]
            LI ["CheckBox" => ctx.Link CheckBox]
            LI ["TextBenchmark" => ctx.Link TextBenchmark]
            LI ["TodoList" => ctx.Link TodoList]
            LI ["SimpleText" => ctx.Link SimpleText]
            LI ["MouseChase" => ctx.Link MouseChase]
            LI ["BarChart" => ctx.Link BarChart]
        ]

    let page title control =
        Skin.WithTemplate title <| fun ctx ->
            [
                Div [Text title]
                Div [] -< [Id "main"]
                Links ctx
                Div [control]
            ]

    let PhonePage =
        page "PhoneExample" (new Controls.EntryPointPhone())
    let CommentPage =
        page "CommentBox" (new Controls.EntryPointComment())
    let CBPage =
        page "CheckBox" (new Controls.EntryPointCB())
    let TBPage =
        page "TextBenchmark" (new Controls.EntryPointTB())
    let TodoPage =
        page "TodoList" (new Controls.EntryPointTodo())
    let SimpleTextPage =
        page "SimpleText" (new Controls.EntryPointSimpleText())
    let MousePage =
        page "MouseChase" (new Controls.EntryPointMouseChase())
    let BarChartPage =
        page "BarChart" (new Controls.EntryPointBarChart())

    let Main =
        Sitelet.Sum [
            Sitelet.Content "/" Phone PhonePage
            Sitelet.Content "/Comment" Comment CommentPage
            Sitelet.Content "/Checkbox" CheckBox CBPage
            Sitelet.Content "/Textbox" TextBenchmark TBPage
            Sitelet.Content "/TodoList" TodoList TodoPage
            Sitelet.Content "/SimpleText" SimpleText SimpleTextPage
            Sitelet.Content "/MouseChase" MouseChase MousePage
            Sitelet.Content "/BarChart" BarChart BarChartPage
        ]

[<Sealed>]
type Website() =
    interface IWebsite<Action> with
        member this.Sitelet = Site.Main
        member this.Actions = [Phone ; Comment ; CheckBox; TextBenchmark]

type Global() =
    inherit System.Web.HttpApplication()

    member g.Application_Start(sender: obj, args: System.EventArgs) =
        ()

[<assembly: Website(typeof<Website>)>]
do ()
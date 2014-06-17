﻿[<ReflectedDefinition>]
module IntelliFactory.WebSharper.UI.Next.ReactiveCollection

open IntelliFactory.WebSharper.UI.Next.Reactive

module RVa = Reactive.Var
module RVi = Reactive.View
module RO = Reactive.Observation

module ReactiveCollection =
    type VarKey = int
    type MapTy<'T> = Map<VarKey, Var<'T>>
    type ReactiveCollection<'T> =
        { InnerMap : Var<MapTy<'T>> ; InnerMapView : View<MapTy<'T>> }

    /// Add a variable to the reactive collection, triggering a re-render
    let AddVar (coll : ReactiveCollection<'T>) (v : Var<'T>) =
        let map = coll.InnerMapView |> RVi.Observe |> RO.Value
        let map' = Map.add (RVa.GetKey v) v map
        // For now...
        RVi.Sink
            (fun _ ->
                let mapVal = coll.InnerMapView |> RVi.Observe |> RO.Value
                RVa.Set coll.InnerMap mapVal) (RVi.Create v)
        RVa.Set coll.InnerMap map'

    // Ideally this would be inside CreateReactiveCollection, but we can't
    // have generic inner functions inside [<JS>] tags...
    let rec addVars coll =
        function
        | [] -> ()
        | x :: xs ->
            AddVar coll x
            addVars coll xs

    let CreateReactiveCollection (vars : Var<'T> list) =
        let (map : MapTy<'T>) = Map.empty
        let mapVar = RVa.Create map
        let mapView = RVi.Create mapVar
        let coll = { InnerMap = mapVar ; InnerMapView = mapView }
        addVars coll vars
        coll

 /// Removes a variable from the reactive collection, triggering a re-render
    let RemoveVar (coll : ReactiveCollection<'T>) (v : Var<'T>) =
        let map = coll.InnerMapView |> RVi.Observe |> RO.Value
        let map' = Map.remove (RVa.GetKey v) map
        RVa.Set coll.InnerMap map'

    let ViewCollection (coll : ReactiveCollection<'T>) =
        coll.InnerMapView 
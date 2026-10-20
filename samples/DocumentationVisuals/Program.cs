using TheSingularityWorkshop.Workshop.Gui;
using TheSingularityWorkshop.Workshop.Gui.Svg;
using System.Xml.Linq;

var output = args.Length > 0
    ? Path.GetFullPath(args[0])
    : Path.GetFullPath("docs/assets/fsm-cos-preflight.svg");

var scene = GuiBuilder.Create("svg-scene", "fsm-cos-preflight")
    .Text("FSM_COS validation and composition path")
    .Property("description",
        "Generated with GUI.SVG from a controlled scene. Shows full dependency-closure preflight, identity and dependency contract validation, optional schedule validation, iterative dependency-first loading, bounded arbitration, host handoff, and distinct failure boundaries. Connector motion is explanatory and honors reduced-motion preferences. It is a contract map, not a live execution trace.");

scene.Child("svg-rect", "background", node => node
    .Property("width", "100").Property("height", "56.25")
    .Property("fill", "#0b1725"));

scene.Child("svg-defs", "definitions", defs => defs
    .Child("svg-marker", "arrowhead", marker => marker
        .Property("refX", "5").Property("refY", "5")
        .Property("markerWidth", "3").Property("markerHeight", "3")
        .Property("orient", "auto")
        .Child("svg-polygon", "arrowhead-shape", polygon => polygon
            .Property("points", "0,0 10,5 0,10").Property("fill", "#7ee0bd"))));

scene.Child("svg-text", "heading", text => text
    .Text("FSM_COS  |  VALIDATION / COMPOSITION / HANDOFF")
    .Property("x", "3").Property("y", "3.2")
    .Property("font-family", "sans-serif").Property("font-size", "2.1")
    .Property("font-weight", "700").Property("fill", "#eef6ff"));

scene.Child("svg-rect", "manifest-box", node => node
    .Property("x", "3").Property("y", "6").Property("width", "18").Property("height", "8")
    .Property("rx", "1").Property("fill", "#173b4c").Property("stroke", "#7ee0bd").Property("stroke-width", "0.3"));
scene.Child("svg-text", "manifest-label", text => text
    .Text("RuntimeManifest").Property("x", "12").Property("y", "10.5")
    .Property("text-anchor", "middle").Property("font-family", "sans-serif").Property("font-size", "1.45").Property("fill", "#eef6ff"));

scene.Child("svg-rect", "roots-box", node => node
    .Property("x", "25").Property("y", "6").Property("width", "20").Property("height", "8")
    .Property("rx", "1").Property("fill", "#173b4c").Property("stroke", "#7ee0bd").Property("stroke-width", "0.3"));
scene.Child("svg-text", "roots-label", text => text
    .Text("Resolve root IDs").Property("x", "35").Property("y", "9.4")
    .Property("text-anchor", "middle").Property("font-family", "sans-serif").Property("font-size", "1.5").Property("fill", "#eef6ff"));
scene.Child("svg-text", "roots-sub-label", text => text
    .Text("and exact versions").Property("x", "35").Property("y", "11.5")
    .Property("text-anchor", "middle").Property("font-family", "sans-serif").Property("font-size", "1.3").Property("fill", "#d6e7f4"));

scene.Child("svg-rect", "identity-box", node => node
    .Property("x", "49").Property("y", "6").Property("width", "22").Property("height", "8")
    .Property("rx", "1").Property("fill", "#302b58").Property("stroke", "#b9a7ff").Property("stroke-width", "0.3"));
scene.Child("svg-text", "identity-label", text => text
    .Text("Validate bundle identity").Property("x", "60").Property("y", "9.4")
    .Property("text-anchor", "middle").Property("font-family", "sans-serif").Property("font-size", "1.4").Property("fill", "#eef6ff"));
scene.Child("svg-text", "identity-sub-label", text => text
    .Text("descriptor + dependency requests").Property("x", "60").Property("y", "11.5")
    .Property("text-anchor", "middle").Property("font-family", "sans-serif").Property("font-size", "1.05").Property("fill", "#d6e7f4"));

scene.Child("svg-rect", "schedule-box", node => node
    .Property("x", "75").Property("y", "6").Property("width", "22").Property("height", "8")
    .Property("rx", "1").Property("fill", "#302b58").Property("stroke", "#b9a7ff").Property("stroke-width", "0.3"));
scene.Child("svg-text", "schedule-label", text => text
    .Text("Optional schedule check").Property("x", "86").Property("y", "9.4")
    .Property("text-anchor", "middle").Property("font-family", "sans-serif").Property("font-size", "1.35").Property("fill", "#eef6ff"));
scene.Child("svg-text", "schedule-sub-label", text => text
    .Text("closure + edges").Property("x", "86").Property("y", "11.5")
    .Property("text-anchor", "middle").Property("font-family", "sans-serif").Property("font-size", "1.25").Property("fill", "#d6e7f4"));

scene.Child("svg-line", "manifest-to-roots", line => line
    .Property("x1", "21").Property("y1", "10").Property("x2", "24").Property("y2", "10")
    .Property("stroke", "#7ee0bd").Property("stroke-width", "0.45").Property("class", "flow-line").Property("marker-end", "url(#arrowhead)"));
scene.Child("svg-line", "roots-to-identity", line => line
    .Property("x1", "45").Property("y1", "10").Property("x2", "48").Property("y2", "10")
    .Property("stroke", "#7ee0bd").Property("stroke-width", "0.45").Property("class", "flow-line").Property("marker-end", "url(#arrowhead)"));
scene.Child("svg-line", "identity-to-schedule", line => line
    .Property("x1", "71").Property("y1", "10").Property("x2", "74").Property("y2", "10")
    .Property("stroke", "#7ee0bd").Property("stroke-width", "0.45").Property("class", "flow-line").Property("marker-end", "url(#arrowhead)"));

scene.Child("svg-rect", "load-box", node => node
    .Property("x", "75").Property("y", "21").Property("width", "22").Property("height", "8")
    .Property("rx", "1").Property("fill", "#173b4c").Property("stroke", "#7ee0bd").Property("stroke-width", "0.3"));
scene.Child("svg-text", "load-label", text => text
    .Text("Load dependencies first").Property("x", "86").Property("y", "24.4")
    .Property("text-anchor", "middle").Property("font-family", "sans-serif").Property("font-size", "1.35").Property("fill", "#eef6ff"));
scene.Child("svg-text", "load-sub-label", text => text
    .Text("then dependents").Property("x", "86").Property("y", "26.5")
    .Property("text-anchor", "middle").Property("font-family", "sans-serif").Property("font-size", "1.25").Property("fill", "#d6e7f4"));

scene.Child("svg-line", "schedule-to-load", line => line
    .Property("x1", "86").Property("y1", "14").Property("x2", "86").Property("y2", "20")
    .Property("stroke", "#7ee0bd").Property("stroke-width", "0.45").Property("class", "flow-line").Property("marker-end", "url(#arrowhead)"));

scene.Child("svg-rect", "arbitration-box", node => node
    .Property("x", "49").Property("y", "21").Property("width", "22").Property("height", "8")
    .Property("rx", "1").Property("fill", "#173b4c").Property("stroke", "#7ee0bd").Property("stroke-width", "0.3"));
scene.Child("svg-text", "arbitration-label", text => text
    .Text("Bounded arbitration").Property("x", "60").Property("y", "24.4")
    .Property("text-anchor", "middle").Property("font-family", "sans-serif").Property("font-size", "1.4").Property("fill", "#eef6ff"));
scene.Child("svg-text", "arbitration-sub-label", text => text
    .Text("stop when stable or fail").Property("x", "60").Property("y", "26.5")
    .Property("text-anchor", "middle").Property("font-family", "sans-serif").Property("font-size", "1.15").Property("fill", "#d6e7f4"));
scene.Child("svg-line", "load-to-arbitration", line => line
    .Property("x1", "75").Property("y1", "25").Property("x2", "72").Property("y2", "25")
    .Property("stroke", "#7ee0bd").Property("stroke-width", "0.45").Property("class", "flow-line").Property("marker-end", "url(#arrowhead)"));

scene.Child("svg-rect", "assembly-box", node => node
    .Property("x", "25").Property("y", "21").Property("width", "20").Property("height", "8")
    .Property("rx", "1").Property("fill", "#173b4c").Property("stroke", "#7ee0bd").Property("stroke-width", "0.3"));
scene.Child("svg-text", "assembly-label", text => text
    .Text("RuntimeAssembly").Property("x", "35").Property("y", "24.4")
    .Property("text-anchor", "middle").Property("font-family", "sans-serif").Property("font-size", "1.4").Property("fill", "#eef6ff"));
scene.Child("svg-text", "assembly-sub-label", text => text
    .Text("only on success").Property("x", "35").Property("y", "26.5")
    .Property("text-anchor", "middle").Property("font-family", "sans-serif").Property("font-size", "1.25").Property("fill", "#d6e7f4"));
scene.Child("svg-line", "arbitration-to-assembly", line => line
    .Property("x1", "49").Property("y1", "25").Property("x2", "46").Property("y2", "25")
    .Property("stroke", "#7ee0bd").Property("stroke-width", "0.45").Property("class", "flow-line").Property("marker-end", "url(#arrowhead)"));

scene.Child("svg-rect", "host-box", node => node
    .Property("x", "3").Property("y", "21").Property("width", "18").Property("height", "8")
    .Property("rx", "1").Property("fill", "#173b4c").Property("stroke", "#7ee0bd").Property("stroke-width", "0.3"));
scene.Child("svg-text", "host-label", text => text
    .Text("Host owns execution").Property("x", "12").Property("y", "24.4")
    .Property("text-anchor", "middle").Property("font-family", "sans-serif").Property("font-size", "1.25").Property("fill", "#eef6ff"));
scene.Child("svg-text", "host-sub-label", text => text
    .Text("FSM_COS hands off").Property("x", "12").Property("y", "26.5")
    .Property("text-anchor", "middle").Property("font-family", "sans-serif").Property("font-size", "1.15").Property("fill", "#d6e7f4"));
scene.Child("svg-line", "assembly-to-host", line => line
    .Property("x1", "25").Property("y1", "25").Property("x2", "22").Property("y2", "25")
    .Property("stroke", "#7ee0bd").Property("stroke-width", "0.45").Property("class", "flow-line").Property("marker-end", "url(#arrowhead)"));

scene.Child("svg-rect", "preflight-failure-box", node => node
    .Property("x", "3").Property("y", "36").Property("width", "44").Property("height", "10")
    .Property("rx", "1").Property("fill", "#3a202c").Property("stroke", "#ff8c9d").Property("stroke-width", "0.3"));
scene.Child("svg-text", "preflight-failure-label", text => text
    .Text("Preflight rejection").Property("x", "25").Property("y", "40")
    .Property("text-anchor", "middle").Property("font-family", "sans-serif").Property("font-size", "1.5").Property("font-weight", "700").Property("fill", "#fff1f3"));
scene.Child("svg-text", "preflight-failure-sub-label", text => text
    .Text("Bad identity, missing dependency, cycle, or schedule mismatch: reject before Load()").Property("x", "25").Property("y", "42.5")
    .Property("text-anchor", "middle").Property("font-family", "sans-serif").Property("font-size", "1.15").Property("fill", "#ffd5dc"));

scene.Child("svg-rect", "runtime-failure-box", node => node
    .Property("x", "53").Property("y", "36").Property("width", "44").Property("height", "10")
    .Property("rx", "1").Property("fill", "#3a202c").Property("stroke", "#ff8c9d").Property("stroke-width", "0.3"));
scene.Child("svg-text", "runtime-failure-label", text => text
    .Text("Runtime failure").Property("x", "75").Property("y", "40")
    .Property("text-anchor", "middle").Property("font-family", "sans-serif").Property("font-size", "1.5").Property("font-weight", "700").Property("fill", "#fff1f3"));
scene.Child("svg-text", "runtime-failure-sub-label", text => text
    .Text("Load / arbitration / non-convergence: no assembly").Property("x", "75").Property("y", "42.5")
    .Property("text-anchor", "middle").Property("font-family", "sans-serif").Property("font-size", "1.1").Property("fill", "#ffd5dc"));

scene.Child("svg-text", "stage-note", text => text
    .Text("LoadPlan stages are metadata; they do not automatically defer loading. Load failures do not roll back prior side effects.")
    .Property("x", "3").Property("y", "51.5")
    .Property("font-family", "sans-serif").Property("font-size", "1.25").Property("fill", "#d6e7f4"));

var svg = new SvgGuiRenderer().Render(scene.Build(), width: 1600, height: 900);

// The pinned GUI.SVG renderer emits the SVG scene; this small, controlled post-pass
// adds CSS motion plus a reduced-motion fallback without changing the scene contract.
XNamespace svgNamespace = "http://www.w3.org/2000/svg";
var document = XDocument.Parse(svg);
document.Root!.Add(new XElement(svgNamespace + "style", """
@keyframes fsm-cos-flow { to { stroke-dashoffset: -2.2; } }
.flow-line { stroke-dasharray: 0.75 0.45; animation: fsm-cos-flow 2.8s linear infinite; }
@media (prefers-reduced-motion: reduce) {
  .flow-line { animation: none !important; stroke-dasharray: none !important; }
}
"""));
var livingSvg = document.ToString(SaveOptions.DisableFormatting);
Directory.CreateDirectory(Path.GetDirectoryName(output)!);
File.WriteAllText(output, livingSvg + Environment.NewLine);
Console.WriteLine($"Generated FSM_COS living preflight visual with GUI.SVG: {output}");

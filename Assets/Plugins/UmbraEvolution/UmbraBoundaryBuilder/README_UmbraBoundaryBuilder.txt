Umbra Boundary Builder v2.4.0
-----------------
As always, if you find any bugs or you have questions/comments, head over to www.umbraevolution.com and use my contact form, or send an email to umbraevolution@gmail.com and I'll respond as soon as possible.
-----------------

-DESCRIPTION-

Hello! This package is meant to help you make level boundaries as quickly and efficiently as possible.

-----------------

-SPECIAL NOTES-

Newer versions of unity (2019 and later) have changed out gizmos work. Make sure that you have not disabled them when placing nodes, or the asset will not have a chance to rebuild the boundary for any new nodes placed.

-----------------

-SETUP-

1) First thing's first, add a Boundary Builder to your scene. You can do this by using the toolbar Tools->UmbraEvolution->UmbraBoundaryBuilder->Add Boundary Builder.

2) Turns out the first thing is also the last thing for setup this time :D

-----------------

-HOW-TO GUIDE-

1) Click the "Add New Boundary" button in the Boundary Builder inspector. A new boundary object should appear in the inspector and as a child of UmbraBoundaryBuilder.

2) Go to your new boundary and give it a height and width so you can see it being constructed while you're placing nodes.

3) Click the "Start Placing Nodes" button.

4) Start clicking in the scene view to place nodes and watch your boundary be made!

5) When you're finished building the boundary, click the "Generate Mesh" button to generate a mesh to be used in navmesh generation. If you ever modify the boundary, just repeat this step to update the mesh as needed.

-----------------

-ADVANCED NOTES-

1) You can change boundary names and gizmo colours from the BoundaryBuilder object to help you differentiate/organize them.
	1.1) You can permanently display the gizmos in-editor by checking the "Gizmos Always On" box on the UmbraBoundryBuilder object's inspector.
	1.2) You can enable physical gizmos that are created as objects in the scene for testing purposes. Don't forget to disable them!

2) Adding a mesh collider component to a boundary after you've generated a mesh will work - but there isn't any reason to since you have perfectly alligned box colliders anyway.

3) You can move nodes the same way as any object in the scene and see the boundary update in real time (but not the mesh - you'll need to re-generate it).

4) Deleting/inserting nodes is fine, everything will update accordingly.
	4.1) Note that nodes are connected in the order they appear within the hierarchy, so changing that order will change the boundaries drastically.

5) When in doubt, increase the height of the boundary, and change the VerticalOffset to suck it down into the surface you're building the boundary on. This is the easiest way to get a solid boundary that the player won't be able to glitch out of easily (a thicker boundary can help as well).

6) The biggest culprit of nodes that end up floating during placement is large trigger colliders. Use the Placeable Layers LayerMask so that nodes are only being placed on relevant surfaces (such as terrain if you have a unique terrain layer).

-----------------

-INSPECTOR BREAKDOWN-

1) Boundary Builder:
*Use Physical Gizmos Toggle: When checked, created objects in the scene that look similar to the in-editor gizmos. Will persist until disabled, even into builds!
**Physical Gizmo Material: Is used by the physical gizmo. I recommend choosing something that supports transparency and setting the Material.color property.
*Gizmos Always On Toggle: When checked, always displays all boundary gizmos, even when not selected.
*Add New Boundary Button: Adds a new boundary as a child of the Boundary Builder and updates the display accordingly.
**Go To Button: Selects the boundary in question in the scene.
**Name Field: Used to rename boundaries (renames the GameObject in the scene as well).
**Colour Field: Used to adjust the colour of a boundary's gizmo without having to select it.
**Delete Button: Used to quickly delete the boundary in question.

2) Boundary:
*Boundary Colour: The colour of the boundary gizmo.
*Node Colour: The colour of the node gizmo.
*Closed Loop: If true, a section of boundary will be generated between the last and first nodes.
*Use Box Colliders: If true, all box colliders will be enabled. If false, all box colliders will be disabled.
*Physics Layer: The physics layer that this boundary should live on. Affects collision interaction with the boundary.
*Boundary Thickness: How thick the box colliders and generated mesh of the boundary will be. Represented by the boundary gizmo.
*Boundary Height: How tall each section of the boundary will be. Represented by the boundary gizmo.
*Vertical Offset: How far, up or down, the boundary will be adjusted compared to the default.
*Boundary Mesh Material: Applied to the boundary mesh when generated. Typically something invisible.
*Placeable Layers: The layers that nodes will be set on via raycast interactions when placing nodes.
*Query Trigger Interaction: Determines how raycasts deal with triggers when placing nodes. See Unity documentation for more information.
*Placing Nodes (Not Interactible): Indicates whether or not node placement is occuring.
*Start Placing Nodes Button: Starts node placement and takes over editor functionality.
**Stop Placing Nodes Button: Stops the node placing process and returns editor control back to normal.
*Generate Mesh Button: Generates a mesh that matches the boundary gizmos exactly. Should be invisible and will be used for baking navmesh.
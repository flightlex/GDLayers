using GdLayers.Attributes;
using GdLayers.Constants;
using System;

namespace GdLayers.Enums;

[Flags]
public enum GdObjectType : uint
{
    // blocks
    [Title("Solid Blocks")]
    [ResourceReference(ResourceTypeConstants.GdObjectGroup.Image, "SolidBlocks")]
    [ResourceReference(ResourceTypeConstants.GdObjectGroup.Description, "SolidBlocks_Description")]
    SolidBlock = 1,

    [Title("Non-Solid Blocks")]
    [ResourceReference(ResourceTypeConstants.GdObjectGroup.Image, "NonSolidBlocks")]
    [ResourceReference(ResourceTypeConstants.GdObjectGroup.Description, "NonSolidBlocks_Description")]
    NonSolidBlock = 2,

    // spikes
    [Title("Solid Spikes")]
    [ResourceReference(ResourceTypeConstants.GdObjectGroup.Image, "SolidSpikes")]
    [ResourceReference(ResourceTypeConstants.GdObjectGroup.Description, "SolidSpikes_Description")]
    SolidSpike = 4,

    [Title("Non-Solid Spikes")]
    [ResourceReference(ResourceTypeConstants.GdObjectGroup.Image, "NonSolidSpikes")]
    [ResourceReference(ResourceTypeConstants.GdObjectGroup.Description, "NonSolidSpikes_Description")]
    NonSolidSpike = 8,

    // hazards
    [Title("Animated Hazards")]
    [ResourceReference(ResourceTypeConstants.GdObjectGroup.Image, "AnimatedHazards")]
    [ResourceReference(ResourceTypeConstants.GdObjectGroup.Description, "AnimatedHazards_Description")]
    AnimatedHazard = 16,

    [Title("Other Hazards")]
    [ResourceReference(ResourceTypeConstants.GdObjectGroup.Image, "OtherHazards")]
    [ResourceReference(ResourceTypeConstants.GdObjectGroup.Description, "OtherHazards_Description")]
    OtherHazard = 32,

    // deco
    [Title("Outline Decorations")]
    [ResourceReference(ResourceTypeConstants.GdObjectGroup.Image, "OutlineDecorations")]
    [ResourceReference(ResourceTypeConstants.GdObjectGroup.Description, "OutlineDecorations_Description")]
    OutlineDecoration = 64,

    [Title("Filler Decorations")]
    [ResourceReference(ResourceTypeConstants.GdObjectGroup.Image, "FillerDecorations")]
    [ResourceReference(ResourceTypeConstants.GdObjectGroup.Description, "FillerDecorations_Description")]
    FillerDecoration = 128,

    [Title("Animated Decorations")]
    [ResourceReference(ResourceTypeConstants.GdObjectGroup.Image, "AnimatedDecorations")]
    [ResourceReference(ResourceTypeConstants.GdObjectGroup.Description, "AnimatedDecorations_Description")]
    AnimatedDecoration = 256,

    [Title("Pixel Decorations")]
    [ResourceReference(ResourceTypeConstants.GdObjectGroup.Image, "PixelDecorations")]
    [ResourceReference(ResourceTypeConstants.GdObjectGroup.Description, "PixelDecorations_Description")]
    PixelDecoration = 512,

    [Title("Animated Pixel Decorations")]
    [ResourceReference(ResourceTypeConstants.GdObjectGroup.Image, "AnimatedPixelDecorations")]
    [ResourceReference(ResourceTypeConstants.GdObjectGroup.Description, "AnimatedPixelDecorations_Description")]
    AnimatedPixelDecoration = 1024,

    [Title("Glow Decorations")]
    [ResourceReference(ResourceTypeConstants.GdObjectGroup.Image, "GlowDecorations")]
    [ResourceReference(ResourceTypeConstants.GdObjectGroup.Description, "GlowDecorations_Description")]
    GlowDecoration = 2048,

    [Title("Other Decorations")]
    [ResourceReference(ResourceTypeConstants.GdObjectGroup.Image, "OtherDecorations")]
    [ResourceReference(ResourceTypeConstants.GdObjectGroup.Description, "OtherDecorations_Description")]
    OtherDecoration = 4096,

    // gameplay
    [Title("Launching Pads")]
    [ResourceReference(ResourceTypeConstants.GdObjectGroup.Image, "LaunchingPads")]
    [ResourceReference(ResourceTypeConstants.GdObjectGroup.Description, "LaunchingPads_Description")]
    LaunchingPad = 8_192,

    [Title("Orbs")]
    [ResourceReference(ResourceTypeConstants.GdObjectGroup.Image, "Orbs")]
    [ResourceReference(ResourceTypeConstants.GdObjectGroup.Description, "Orbs_Description")]
    Orb = 16_384,

    [Title("Gravity Portals")]
    [ResourceReference(ResourceTypeConstants.GdObjectGroup.Image, "GravityPortals")]
    [ResourceReference(ResourceTypeConstants.GdObjectGroup.Description, "GravityPortals_Description")]
    GravityPortal = 32_768,

    [Title("Gamemode Portals")]
    [ResourceReference(ResourceTypeConstants.GdObjectGroup.Image, "GamemodePortals")]
    [ResourceReference(ResourceTypeConstants.GdObjectGroup.Description, "GamemodePortals_Description")]
    GamemodePortal = 65_536,

    [Title("Size Portals")]
    [ResourceReference(ResourceTypeConstants.GdObjectGroup.Image, "SizePortals")]
    [ResourceReference(ResourceTypeConstants.GdObjectGroup.Description, "SizePortals_Description")]
    SizePortal = 131_072,

    [Title("Other Portals")]
    [ResourceReference(ResourceTypeConstants.GdObjectGroup.Image, "OtherPortals")]
    [ResourceReference(ResourceTypeConstants.GdObjectGroup.Description, "OtherPortals_Description")]
    OtherPortal = 262_144,

    // other gameplay
    [Title("Speed Modifiers")]
    [ResourceReference(ResourceTypeConstants.GdObjectGroup.Image, "SpeedModifiers")]
    [ResourceReference(ResourceTypeConstants.GdObjectGroup.Description, "SpeedModifiers_Description")]
    SpeedModifier = 524_288,

    [Title("Special Blocks")]
    [ResourceReference(ResourceTypeConstants.GdObjectGroup.Image, "SpecialBlocks")]
    [ResourceReference(ResourceTypeConstants.GdObjectGroup.Description, "SpecialBlocks_Description")]
    SpecialBlock = 1_048_576,

    // saw
    [Title("Saws")]
    [ResourceReference(ResourceTypeConstants.GdObjectGroup.Image, "Saws")]
    [ResourceReference(ResourceTypeConstants.GdObjectGroup.Description, "Saws_Description")]
    Saw = 2_097_152,

    // other
    [Title("Collectible Objects")]
    [ResourceReference(ResourceTypeConstants.GdObjectGroup.Image, "Collectibles")]
    [ResourceReference(ResourceTypeConstants.GdObjectGroup.Description, "Collectibles_Description")]
    Collectible = 4_194_304,

    [Title("Triggers")]
    [ResourceReference(ResourceTypeConstants.GdObjectGroup.Image, "Triggers")]
    [ResourceReference(ResourceTypeConstants.GdObjectGroup.Description, "Triggers_Description")]
    Trigger = 8_388_608
}

﻿using InDappledGroves.BlockEntities;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace InDappledGroves.Blocks
{
    class IDGSawHorse : Block
    {

        //public override void OnBlockPlaced(IWorldAccessor world, BlockPos blockPos, ItemStack byItemStack = null)
        //{
        //    base.OnBlockPlaced(world, blockPos, byItemStack);
        //}
        public override void OnLoaded(ICoreAPI api)
        {
            base.OnLoaded(api);
        }

        public override bool TryPlaceBlock(IWorldAccessor world, IPlayer byPlayer, ItemStack itemstack, BlockSelection blockSel, ref string failureCode)
        {
            return base.TryPlaceBlock(world, byPlayer, itemstack, blockSel, ref failureCode);
        }

        public override void OnBlockBroken(IWorldAccessor world, BlockPos pos, IPlayer byPlayer, float dropQuantityMultiplier = 1)
        {
            base.OnBlockBroken(world, pos, byPlayer, dropQuantityMultiplier);
        }
        public override void OnNeighbourBlockChange(IWorldAccessor world, BlockPos pos, BlockPos neibpos)
        {
            bool posSawhorse = api.World.BlockAccessor.GetBlockEntity(pos) is not IDGBESawHorse besawhorse;
            bool posneibSawHorse = api.World.BlockAccessor.GetBlockEntity(neibpos) is not IDGBESawHorse neibesawhorse2;
            string side = api.World.BlockAccessor.GetBlock(pos).Variant["side"];
            string neiside = api.World.BlockAccessor.GetBlock(pos).Variant["side"];


            if (posSawhorse && posneibSawHorse) base.OnNeighbourBlockChange(world, pos, neibpos);
            
            if (!posSawhorse && !posneibSawHorse)
            {
                besawhorse = api.World.BlockAccessor.GetBlockEntity(pos) as IDGBESawHorse;
                neibesawhorse2 = api.World.BlockAccessor.GetBlockEntity(neibpos) as IDGBESawHorse;
                System.Diagnostics.Debug.WriteLine("Checkpoint " + api.World.Side);
                if (!besawhorse.isPaired && !neibesawhorse2.isPaired && (neiside == side)) 
                {
                    besawhorse.CreateSawhorseStation(neibpos);
                    neibesawhorse2.conBlock = pos;
                    neibesawhorse2.pairedBlock = pos;
                    neibesawhorse2.isPaired = true;
                    neibesawhorse2.isConBlock = false;
                    Block block = api.World.BlockAccessor.GetBlock(neibpos);
                    if (neibesawhorse2.conBlock == besawhorse.conBlock) api.World.BlockAccessor.ExchangeBlock(api.World.GetBlock(this.CodeWithVariant("side", block.Variant["side"])).BlockId, pos);
                }
            }
            base.OnNeighbourBlockChange(world, pos, neibpos);
        }

        public override void OnBlockRemoved(IWorldAccessor world, BlockPos pos)
        {
            IDGBESawHorse besawhorse;
            IDGBESawHorse besawhorse2;
            System.Diagnostics.Debug.WriteLine("OnBlockRemoved on " + api.Side);
            if (api.World.BlockAccessor.GetBlockEntity(pos) is IDGBESawHorse)
            {
                besawhorse = api.World.BlockAccessor.GetBlockEntity(pos) as IDGBESawHorse;
                if (besawhorse.isPaired)
                {
                    if (api.World.BlockAccessor.GetBlockEntity(besawhorse.pairedBlock) is IDGBESawHorse)
                    {
                        besawhorse2 = api.World.BlockAccessor.GetBlockEntity(besawhorse.pairedBlock) as IDGBESawHorse;

                        besawhorse2.isConBlock = false;
                        besawhorse2.conBlock = null;
                        besawhorse2.isPaired = false;
                        besawhorse2.pairedBlock = null;
                        besawhorse2.MarkDirty(true);
                        if (!besawhorse2.Inventory[0].Empty)
                        {
                            api.World.SpawnItemEntity(besawhorse.Inventory[0].TakeOutWhole(), pos.ToVec3d(), new Vec3d(0f, 0.5f, 0f));
                        }
                    }
                }
            }
            base.OnBlockRemoved(world, pos);
        }
        //public override bool OnBlockInteractStart(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel)
        //{
            
        //    ItemSlot playerSlot = byPlayer.InventoryManager.ActiveHotbarSlot;
        //    string failureCode = "";
        //    BlockSelection placeSel = blockSel.Clone();
        //    BlockPos placePosition = placeSel.Position.Offset(blockSel.Face);
        //    if (world.BlockAccessor.GetBlockEntity(blockSel.Position) is not IDGBESawHorse besawhorse) return base.OnBlockInteractStart(world, byPlayer, blockSel);
        //    if (!playerSlot.Empty && playerSlot.Itemstack.Collectible is IDGSawHorse)
        //    {
        //        if (besawhorse.isPaired == false && CanPlaceBlock(world, byPlayer, placeSel, ref failureCode))
        //        {
        //              besawhorse.CreateSawhorseStation(placePosition);
        //        }

        //    }
        //    else
        //    {
        //        if (besawhorse.conBlock != null) (api.World.BlockAccessor.GetBlockEntity(besawhorse.conBlock) as IDGBESawHorse).OnInteract(byPlayer, blockSel);
        //    }
        //    return false;
        //}

    }

    
}
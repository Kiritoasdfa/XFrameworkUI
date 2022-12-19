using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFramework
{
    public class UICharacterEquipList : UI, IAwake, ILoopScrollRectProvide<UIEquipItem>
    {
        private List<int> equipList = new List<int>(50);

        public void Initialize()
        {
            for (int i = 0; i < 50; i++)
            {
                this.equipList.Add(10000 + i);
            }

            var loopRect = this.GetLoopScrollRect();
            loopRect.SetProvideData(UIPathSet.UIEquipItem, this);
            loopRect.SetTotalCount(this.equipList.Count);
            loopRect.RefillCells();
        }

        protected override void OnClose()
        {
            this.equipList.Clear();
            base.OnClose();
        }

        void ILoopScrollRectProvide<UIEquipItem>.ProvideData(UIEquipItem ui, int index)
        {
            var equipId = this.equipList[index];
            ObjectHelper.Awake(ui, equipId);
        }
    }
}

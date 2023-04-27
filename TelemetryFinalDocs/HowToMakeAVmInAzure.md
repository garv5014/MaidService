# Setting Supabase On An Azure Ubuntu Vm With A Reverse Proxy.
## Azure Vm.
### Making The Resource Group.
1. Sign into your azure portal. From the home page create a resource group by hitting the create resource button in the top left. !["Create Resource Photo"](./images/CreateResourceButton.png)
1. Once you are in the Create Resource page use the search function and look up Resource Group. Once you have searched this there will be an option that says resource group. Click the create button and hit Resource Group. !["Resource Group Photo"](./images/SearchAndCreateResourceGroup.png)
1. It will take you through the work flow of setting up the resource group. If you don't know what to name the group or what supscription or region to put it under/in talk to your team to figure out what subscription you can use and if the region of the group matter. The name of the resource group doesn't really matter as long as it is unique in the organization. You won't need to add any tags for this group. Once the group is named and the subscription and region have been choose you can hit next until you make it to the create page and then hit create.!["Resource Creation"](./images/SupabaseResourceGroupCreation.png)
1. Once the Resource Group is created you should be able to find it on the home page.
### Making The Ubuntu Vm
1. We need to make a new resource, but this time we will be making a ubuntu virtual machine instead of a resource group. This will be the same as steps one and two from the Making The Resource Group section above. However, instead of searching resource group in step two you will search virtual machine.!["Virtual Machine Creation"](./images/CreateVirtualMachine.png) Click the create button and then hit virtual machine. This will take you to a similiar work flow as the one for making the resource group. 
1. Once you are inside the work flow under the basics tab you will need to configure the options listed below. 
    - Subscription: This should be the same subscritption as the resource group you made earlier. 
    - Resource Group: This should be the resource group made earlier. 
    - Name: Make a name for your machine. It won't really matter what you call it but make sure it is named so that it can be identified easily later. 
    - Region: Like the resource group the region shouldn't really mattter. If your team doesn't tell you make it the same as the resource group or try and select the region closest to you. 
    - Image: You can choose any image you want as long as it is an Ubuntu Server 20.04 image.
    - Size: This will determine how much machine you get and is the largest factor in determining cost. For our purposes we won't need anything greater then 2 vcpus, and 8GiB memory.
    - Authentication type: Toggle this one to password
    - Username: This will be the username that we will use to log into the machine. Make sure it is secure but something you won't forget.
    - Password/Confirm Password: These will be the password that we will use to sign into the the virtual machine from a remote host. Do Not Forget Them!
    - Public Inbound Ports: We will have this toggled to allow selected ports
    - Select Inbound Ports: For now we will only need port 22 selected. We will be able to expose other ports to the machine once it is up later. 
1. Once you have configured the basics tab you can click the networking tab and there are only two thing we need to configure here. 
    - Virtual Network: If there isn't already one made hit create new under the drop down box and give the v-net a meaningful name and hit create. 
    - Subnet: If you just made the network this should autofill for you. If not using the default supnet should be fine. Check with your team to make sure that the default will be fine for this machine. 
1. After that go to the Review+create tab and it will make sure all the required configuration is there. Once it is done checking and if there aren't any error hit create.

### Exposing Ports And Making A DNS For The Machine
1. After the machine is made you can go to it's resource group and should see something similiar to this. 
![](./images/ResourceGroupWithMachine.png)
Azure made your virtual machine, in this case MaidSupabase, and all the supporting resources for it. 
1. Click on your machine and it will take you to an overview page. On the left side you will see a lot of different views where we can configure different elements of the machine. First we want to give our machine a DNS. We can find this in the overview page if it doesn't have one it will have a place under neither DNS name to click to make one. ![](./images/DNSNameAndPublicIpAddress.png)
You can see the public Ip address here along with the DNS name. This machine already had it's name configured. If it isn't configured hit the option under the DNS name and it will take you and ask you to provide a name. ![](./images/DNSNameCreation.png)
When you type in the name it will tell you if it is valid or not. Once you have a valid name go back the resource group and your machine to see the DNS name field has been changed. 

## Setting Up The Vm 
- For this next part we will be using Visual Studio Code remote connection to connect to our machine. If you don't have Visual Studio Code on the machine(Not the virtual machine but your own) Than go [here](https://code.visualstudio.com/download) and download it to your local machine.